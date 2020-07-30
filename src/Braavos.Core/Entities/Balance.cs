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

        public override string ToString()
        {
            var balance = Amount.ToString();
            balance = $"{(Category == Category.Credit ? "+" : "-")}{balance}";
            balance = $"{balance}{(Type == BalanceType.Cash ? "$" : "")}";
            balance = $"{balance} ({Category})";
            return balance;
        }
    }
}
