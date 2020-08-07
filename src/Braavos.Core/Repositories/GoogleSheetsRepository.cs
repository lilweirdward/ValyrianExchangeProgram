using Braavos.Core.Entities;
using Braavos.Core.Infrastructure;
using Google.Apis.Auth.OAuth2.Requests;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
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
            var request = _sheetsService.Spreadsheets.Values.Get(_gSheetsSpreadsheetId, "VAid!C2:G");
            var response = await request.ExecuteAsync();

            var users = response.Values
                .Where(row => row.Count() == 5) // Avoid rows with no values since it jacks up the API
                .Select(row => new AuthorizedUser
                {
                    NationId = Convert.ToInt32(row[0]),
                    RulerName = row[2].ToString(),
                    UniqueCode = row[4].ToString()
                });

            return users.FirstOrDefault(user => user.UniqueCode == authRequest.UniqueCode && (user.NationId == authRequest.NationId || user.RulerName == authRequest.RulerName));
        }

        public async Task<Account> GetAccountDetails(AuthorizedUser user)
        {
            var request = _sheetsService.Spreadsheets.Values.Get(_gSheetsSpreadsheetId, "VAid!B2:AD");
            var response = await request.ExecuteAsync();

            var data = response.Values
                .Where(row => row.Count() == 29)
                .Select(RowAsUserAndAccount);

            if (!data.TryFirst(x => x.AuthorizedUser.Equals(user), out var authorizedAccount))
                return null;

            var account = authorizedAccount.Account;
            account.PotentialTransactions = GetPotentialTransactions(data.Select(x => x.Account), account);

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
            var credit = Convert.ToInt32(row[19]);
            var debt = Convert.ToInt32(row[20]);

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
                case Role.Collector:
                    balance.Type = BalanceType.Cash;
                    break;
                // Farms and Receivers are always exchanging tech
                case Role.Farm:
                case Role.Receiver:
                    balance.Type = BalanceType.Tech;
                    break;
            }

            return balance;
        }

        private List<Account> GetPotentialTransactions(IEnumerable<Account> allAccounts, Account currentAccount)
        {
            // Exit early if account has no free aid slots
            if (currentAccount.AvailableSlots == 0)
                return new List<Account>();

            // Filter out anyone who doesn't have free slots
            allAccounts = allAccounts.Where(account => account.AvailableSlots > 0);

            // Build the 4 primary lists
            var sendingCash = allAccounts
                .Where(account => account.Role == Role.Buyer || account.Role == Role.Donor)
                .Where(account => account.Balance.Category == Category.Even);
            var receivingCash = allAccounts
                .Where(account => account.Role == Role.Collector || account.Role == Role.Seller || account.Role == Role.ProbationarySeller)
                .Where(account => account.Balance.Category == Category.Even);
            var sendingTech = allAccounts
                .Where(account => account.Role == Role.Farm || account.Role == Role.Seller || account.Role == Role.ProbationarySeller)
                .Where(account => account.Balance.Amount > 0 && account.Balance.Category == Category.Debt);
            var receivingTech = allAccounts
                .Where(account => account.Role == Role.Buyer || account.Role == Role.Receiver)
                .Where(account => account.Balance.Amount > 0 && account.Balance.Category == Category.Credit);

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
        }
    }
}
