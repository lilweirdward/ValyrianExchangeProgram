using Braavos.Core.Entities;
using Braavos.Core.Infrastructure;
using Google.Apis.Auth.OAuth2.Requests;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace Braavos.Core.Repositories
{
    public class GoogleSheetsRepository : IBraavosRepository
    {
        private readonly string _gSheetsSpreadsheetId;
        private readonly SheetsService _sheetsService;

        public GoogleSheetsRepository(IOptions<FunctionOptions> options)
        {
            _gSheetsSpreadsheetId = options.Value.GoogleSheetsSpreadsheetId;

            _sheetsService = new SheetsService(new BaseClientService.Initializer
            {
                ApplicationName = "Braavos",
                ApiKey = options.Value.GoogleSheetsApiKey
            });
        }

        public async Task<AuthorizedUser> Authorize(AuthRequest authRequest)
        {
            var request = _sheetsService.Spreadsheets.Values.Get(_gSheetsSpreadsheetId, "VAid!B2:F");
            var response = await request.ExecuteAsync();

            var users = response.Values
                .Where(row => row.Count() == 5) // Avoid rows with no values since it jacks up the API
                .Select(row => new AuthorizedUser
                {
                    NationId = Convert.ToInt32(row[0]),
                    RulerName = row[2].ToString(),
                    UniqueCode = row[4].ToString(),
                    DiscordTag = row[3].ToString()
                });

            if (string.IsNullOrEmpty(authRequest.DiscordTag))
                return users.FirstOrDefault(user =>
                    user.UniqueCode == authRequest.UniqueCode &&
                    (user.NationId == authRequest.NationId || user.RulerName.Equals(authRequest.RulerName, StringComparison.InvariantCultureIgnoreCase)));

            return users.FirstOrDefault(user => !string.IsNullOrEmpty(user.DiscordTag) && user.DiscordTag.Equals(authRequest.DiscordTag));
        }

        public async Task<Account> GetAccountDetails(AuthorizedUser user)
        {
            var request = _sheetsService.Spreadsheets.Values.Get(_gSheetsSpreadsheetId, "VAid!A2:AC");
            var response = await request.ExecuteAsync();

            var data = response.Values
                .Where(row => row.Count() == 29)
                .Select(RowAsUserAndAccount);

            if (!data.TryFirst(x => x.AuthorizedUser.Equals(user), out var authorizedAccount))
                return null;

            var account = authorizedAccount.Account;
            account.Metadata = await GetAdditionalMetadata();
            account.PotentialTransactions = await GetPotentialTransactions(data.Select(x => x.Account), account);
            account.AcceptableRecipients = GetAcceptableRecipients(data.Select(x => x.Account), account);
            account.RecentTransactions = await GetRecentTransactions(account);

            // Cross reference to not suggest potential transactions that are also recent (active) transactions
            account.PotentialTransactions = account.PotentialTransactions
                .Where(potential => !account.RecentTransactions.Any(t => t.Status == TransactionStatus.Approved && t.OtherRuler == potential.RulerName))
                .ToList();

            return account;
        }

        private (AuthorizedUser AuthorizedUser, Account Account) RowAsUserAndAccount(IList<object> row)
        {
            var authorizedUser = new AuthorizedUser
            {
                NationId = Convert.ToInt32(row[1]),
                RulerName = row[3].ToString(),
                UniqueCode = row[5].ToString()
            };

            var account = new Account
            {
                Id = Convert.ToInt32(row[1]),
                RulerName = row[3].ToString(),
                NationName = row[2].ToString(),
                Role = row[6].ToString().ToRoleFromCode(),
                Alliance = row[0].ToString(),
                Balance = RowAsBalance(row),
                AvailableSlots = row[15].ToString().ToNullableInt()
            };

            return (authorizedUser, account);
        }

        private Balance RowAsBalance(IList<object> row)
        {
            var credit = int.Parse(row[19].ToString(), NumberStyles.AllowThousands);
            var debt = int.Parse(row[20].ToString(), NumberStyles.AllowThousands);

            // Either credit or debt will always be 0, so the balance amount is always the max of the two
            var balance = new Balance { Amount = Math.Max(credit, debt) };

            if (balance.Amount == 0)
                balance.Category = Category.Even; // If the max is 0 then the account has neither credit nor debt
            else if (balance.Amount == credit)
                balance.Category = Category.Credit;
            else
                balance.Category = Category.Debt;

            var role = row[6].ToString().ToRoleFromCode();
            switch (role)
            {
                // Buyers always show credit as tech, but debt as cash
                case Role.Buyer:
                    balance.Type = balance.Category == Category.Credit ? BalanceType.Tech : BalanceType.Cash;
                    break;
                // Sellers always show credit as cash, but debt as tech
                case Role.Seller:
                case Role.ProbationarySeller:
                    balance.Type = balance.Category == Category.Credit ? BalanceType.Cash : BalanceType.Tech;
                    break;
                // Donors and Collectors are always exchanging cash
                case Role.Donor:
                case Role.TemporaryDonor:
                case Role.Collector:
                case Role.TemporaryCollector:
                    balance.Type = BalanceType.Cash;
                    break;
                // Farms and Receivers are always exchanging tech
                case Role.Farm:
                case Role.TemporaryFarm:
                case Role.Receiver:
                case Role.TemporaryReceiver:
                    balance.Type = BalanceType.Tech;
                    break;
            }

            return balance;
        }

        private async Task<VepMeta> GetAdditionalMetadata()
        {
            var request = _sheetsService.Spreadsheets.Values.Get(_gSheetsSpreadsheetId, "Lists!D5");
            var data = (await request.ExecuteAsync()).Values.FirstOrDefault();

            return data is null ? null : new VepMeta(Convert.ToInt32(data[0]));
        }

        private async Task<List<Account>> GetPotentialTransactions(IEnumerable<Account> allAccounts, Account currentAccount)
        {
            // Exit early if account has no free aid slots
            if (currentAccount.AvailableSlots == 0)
                return new List<Account>();

            // Build the 4 primary lists
            var sendingCash = await QuerySheetsForRulerNames("L1IC");
            var receivingCash = await QuerySheetsForRulerNames("L2CT", "B", 6);
            var sendingTech = await QuerySheetsForRulerNames("L3IT");
            var receivingTech = await QuerySheetsForRulerNames("L4TT");

            // Potential transactions are always the inverse of whichever list the current account is in
            // i.e. an account that is sending cash has potential transactions with nations that are receiving cash
            return currentAccount switch
            {
                var account when sendingCash.Contains(account) => receivingCash.ToList(),
                var account when receivingCash.Contains(account) => sendingCash.ToList(),
                var account when sendingTech.Contains(account) => receivingTech.ToList(),
                var account when receivingTech.Contains(account) => sendingTech.ToList(),
                _ => new List<Account>()
            };

            async Task<IEnumerable<Account>> QuerySheetsForRulerNames(string sheetName, string column = "A", int startingRow = 5)
            {
                var request = _sheetsService.Spreadsheets.Values.Get(_gSheetsSpreadsheetId, $"{sheetName}!{column}{startingRow}:{column}");
                var response = await request.ExecuteAsync();
                return response.Values?
                    .Where(row => row[0] != null)
                    .Select(row => allAccounts.FirstOrDefault(account => account.RulerName == row[0].ToString()))
                    ?? new List<Account>();
            }
        }

        private List<ValidAccount> GetAcceptableRecipients(IEnumerable<Account> allAccounts, Account currentAccount)
        {
            var results = new List<ValidAccount>();

            // Exit early if account has no free aid slots
            if (currentAccount.AvailableSlots == 0)
                return results;

            // Filter out accounts that are inactive or are the current account (can't deal with yourself lol)
            var baseList = allAccounts
                .Where(a => a.Id != currentAccount.Id && a.Role != Role.NotParticipating)
                .Select(a => new Account(a) { Metadata = currentAccount.Metadata });

            // Calculate whether each account should actually be sending the current account aid or not, and return
            foreach (var otherVepAccount in baseList)
            {
                var shouldBeSendingCurrentAccountAid = currentAccount.Role switch
                {
                    // Accounts that send tech (for credit or free) are valid senders for a Buyer
                    Role.Buyer when otherVepAccount.IsSeller() || otherVepAccount.IsFarm() => true,

                    // Accounts that send cash (for credit or free) are valid senders for a Seller
                    Role.Seller when otherVepAccount.IsBuyer() || otherVepAccount.IsDonor() => true,
                    _ => false
                };

                results.Add(new ValidAccount(otherVepAccount) { ShouldBeSendingAid = shouldBeSendingCurrentAccountAid });
            }

            return results;
        }

        private async Task<List<Transaction>> GetRecentTransactions(Account currentAccount, int daysIncluded = 30)
        {
            // Get all the transactions from the Hist tab
            var request = _sheetsService.Spreadsheets.Values.Get(_gSheetsSpreadsheetId, "Hist!A2:Q");
            var data = (await request.ExecuteAsync()).Values.Where(row => row.Count == 17).Select(row => new
            {
                DeclaringRuler = row[0],
                ReceivingRuler = row[1],
                Status = row[2],
                Money = row[3],
                Tech = row[4],
                Soldiers = row[5],
                Start = row[8],
                Type = row[11],
                CT = row[13],
                CC = row[14],
                TC = row[15],
                TT = row[16]
            }).Where(x => x.Type.ToString() == "1");

            // Convert relevant transactions into real objects
            var outgoingTransactions = data
                .Where(x => x.DeclaringRuler.ToString() == currentAccount.RulerName)
                .Where(x => DateTime.Parse(x.Start.ToString()) >= DateTime.Today.AddDays(-daysIncluded))
                .Select(row => new Transaction
                {
                    OtherRuler = row.ReceivingRuler.ToString(),
                    Type = TransactionType.Outgoing,
                    Status = (TransactionStatus) Enum.Parse(typeof(TransactionStatus), row.Status.ToString()),
                    Money = int.Parse(row.Money.ToString(), NumberStyles.AllowThousands),
                    Tech = Convert.ToInt32(row.Tech),
                    Soldiers = Convert.ToInt32(row.Soldiers),
                    AccountChangeType = Category.Credit,
                    SentOn = DateTime.Parse(row.Start.ToString())
                });

            var incomingTransactions = data
                .Where(x => x.ReceivingRuler.ToString() == currentAccount.RulerName)
                .Where(x => DateTime.Parse(x.Start.ToString()) >= DateTime.Today.AddDays(-daysIncluded))
                .Select(row => new Transaction
                {
                    OtherRuler = row.DeclaringRuler.ToString(),
                    Type = TransactionType.Incoming,
                    Status = (TransactionStatus)Enum.Parse(typeof(TransactionStatus), row.Status.ToString()),
                    Money = int.Parse(row.Money.ToString(), NumberStyles.AllowThousands),
                    Tech = Convert.ToInt32(row.Tech),
                    Soldiers = int.Parse(row.Soldiers.ToString(), NumberStyles.AllowThousands),
                    AccountChangeType = Category.Debt,
                    SentOn = DateTime.Parse(row.Start.ToString())
                });

            return outgoingTransactions.MergeWith(incomingTransactions).OrderByDescending(t => t.SentOn).ToList();
        }
    }
}
