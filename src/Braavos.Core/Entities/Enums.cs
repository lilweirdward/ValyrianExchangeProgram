using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Text;

namespace Braavos.Core.Entities
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum Role
    {
        NotParticipating = 0,
        Buyer,
        Seller,
        Donor,
        Farm,
        Collector,
        Receiver,
        ProbationarySeller
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum BalanceType
    {
        Tech,
        Cash
    }

    [JsonConverter(typeof(StringEnumConverter))]
    public enum Category
    {
        Credit,
        Debt,
        Even
    }
}
