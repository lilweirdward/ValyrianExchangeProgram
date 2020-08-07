using System;
using System.Collections.Generic;
using System.Text;

namespace Braavos.Core.Entities
{
    public struct Balance
    {
        public int Amount { get; set; }
        public BalanceType Type { get; set; }
        public Category Category { get; set; }

        public override bool Equals(object obj)
        {
            if (!(obj is Balance otherBalance))
                return false;

            return
                Amount == otherBalance.Amount &&
                Type == otherBalance.Type &&
                Category == otherBalance.Category;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = Amount.GetHashCode();
                hashCode = (hashCode * 397) ^ Type.GetHashCode();
                hashCode = (hashCode * 397) ^ Category.GetHashCode();
                return hashCode;
            }
        }
    }
}
