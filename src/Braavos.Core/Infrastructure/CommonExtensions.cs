using Braavos.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;

namespace Braavos.Core.Infrastructure
{
    public static class CommonExtensions
    {
        public static bool TryFirst<T>(this IEnumerable<T> enumerable, Func<T, bool> filter, out T result)
        {
            result = default;

            foreach (var item in enumerable)
                if (filter(item))
                {
                    result = item;
                    return true;
                }

            return false;
        }

        public static int? ToNullableInt(this string value)
        {
            if (string.IsNullOrEmpty(value))
                return null;

            // At this point I'm fine with whatever default the out param gives me
            int.TryParse(value, out var parsedValue);
            return parsedValue;
        }

        public static IEnumerable<T> MergeWith<T>(this IEnumerable<T> currentList, IEnumerable<T> otherList)
        {
            var result = currentList.ToList();
            result.AddRange(otherList);
            return result;
        }

        public static bool IsUnauthorizedStatusCode(this HttpResponseMessage responseMessage) => responseMessage.StatusCode == HttpStatusCode.Unauthorized;

        public static bool IsNotFoundStatusCode(this HttpResponseMessage responseMessage) => responseMessage.StatusCode == HttpStatusCode.NotFound;

        public static Role ToRoleFromCode(this string value)
        {
            switch (value)
            {
                case "S": return Role.Seller;
                case "B": return Role.Buyer;
                case "D": return Role.Donor;
                case "F": return Role.Farm;
                case "C": return Role.Collector;
                case "R": return Role.Receiver;
                case "N": return Role.ProbationarySeller;
                default:
                    return Role.NotParticipating;
            }
        }

        public static string ToCode(this Role value) => value switch
        {
            Role.Seller => "S",
            Role.Buyer => "B",
            Role.Donor => "D",
            Role.Farm => "F",
            Role.Collector => "C",
            Role.Receiver => "R",
            Role.ProbationarySeller => "N",
            _ => "H"
        };

        public static bool OwesCash(this Account account) => 
            (account.Role == Role.Buyer || account.Role == Role.Donor) &&
            (account.Balance.Amount > 0 && account.Balance.Category == Category.Debt || account.Balance.Category == Category.Even);

        public static bool ExpectsCash(this Account account) =>
            account.IsSeller() && (
                account.HasCreditBalance(amount => amount > 0) ||
                account.HasDebtBalance(amount => amount <= account.Metadata.SellerDebtOverride)) ||
            account.Role == Role.Collector && account.HasCreditBalance(amount => amount == 0) ||
            account.Balance.Category == Category.Even;

        public static bool OwesTech(this Account account) =>
            account.IsSeller() && account.HasDebtBalance(amount => amount > account.Metadata.SellerDebtOverride) ||
            account.IsFarm() && account.HasDebtBalance(amount => amount > 0);

        public static bool ExpectsTech(this Account account) =>
            (account.Role == Role.Buyer || account.Role == Role.Receiver) && account.Balance.Amount > 0 && account.Balance.Category == Category.Credit;

        public static bool IsSeller(this Account account) => account.Role == Role.Seller || account.Role == Role.ProbationarySeller;

        public static bool IsBuyer(this Account account) => account.Role == Role.Buyer;

        public static bool IsFarm(this Account account) => account.Role == Role.Farm;

        public static bool IsDonor(this Account account) => account.Role == Role.Donor;

        public static bool HasDebtBalance(this Account account, Func<int, bool> balanceFunc)
            => account.Balance.Category == Category.Debt && balanceFunc(account.Balance.Amount);

        public static bool HasCreditBalance(this Account account, Func<int, bool> balanceFunc)
            => account.Balance.Category == Category.Credit && balanceFunc(account.Balance.Amount);
    }
}
