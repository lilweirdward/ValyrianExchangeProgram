using Braavos.Core.Entities;
using System;
using System.Collections.Generic;
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

        public static bool IsUnauthorizedStatusCode(this HttpResponseMessage responseMessage) => responseMessage.StatusCode == HttpStatusCode.Unauthorized;

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
    }
}
