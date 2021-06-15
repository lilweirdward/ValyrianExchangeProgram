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
        ProbationarySeller,
        TemporaryDonor,
        TemporaryCollector,
        TemporaryFarm,
        TemporaryReceiver
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

    public enum TransactionType
    {
        Incoming,
        Outgoing
    }

    public enum TransactionStatus
    {
        Pending,
        Approved,
        Expired,
        Intraday
    }
}
