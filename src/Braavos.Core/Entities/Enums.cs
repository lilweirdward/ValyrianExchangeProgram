using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Braavos.Core.Entities
{
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

    public enum BalanceType
    {
        Tech,
        Cash
    }

    public enum Category
    {
        Credit,
        Debt,
        Even
    }
}
