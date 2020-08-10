using System;
using System.Collections.Generic;
using System.Text;

namespace Braavos.Core.Entities
{
    public class Account
    {
        public int Id { get; set; }
        public string RulerName { get; set; }
        public string NationName { get; set; }
        public Role Role { get; set; }
        public string Alliance { get; set; }
        public Balance Balance { get; set; }
        public int? AvailableSlots { get; set; }
        public List<Account> PotentialTransactions { get; set; }

        public override bool Equals(object obj)
        {
            if (!(obj is Account otherAccount))
                return false;

            return
                Id == otherAccount.Id &&
                RulerName == otherAccount.RulerName &&
                NationName == otherAccount.NationName &&
                Role == otherAccount.Role &&
                Alliance == otherAccount.Alliance &&
                Balance.Equals(otherAccount.Balance) &&
                AvailableSlots == otherAccount.AvailableSlots;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = Id.GetHashCode();
                hashCode = (hashCode * 397) ^ RulerName.GetHashCode();
                hashCode = (hashCode * 397) ^ NationName.GetHashCode();
                hashCode = (hashCode * 397) ^ Role.GetHashCode();
                hashCode = (hashCode * 397) ^ Alliance.GetHashCode();
                hashCode = (hashCode * 397) ^ Balance.GetHashCode();
                hashCode = (hashCode * 397) ^ AvailableSlots.GetHashCode();
                return hashCode;
            }
        }
    }
}
