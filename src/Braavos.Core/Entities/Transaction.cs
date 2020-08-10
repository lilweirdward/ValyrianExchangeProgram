using System;
using System.Collections.Generic;
using System.Text;

namespace Braavos.Core.Entities
{
    public class Transaction
    {
        public string OtherRuler { get; set; }
        public TransactionType Type { get; set; }
        public int Money { get; set; }
        public int Tech { get; set; }
        public int Soldiers { get; set; }
        public Category AccountChangeType { get; set; }
        public DateTime SentOn { get; set; }
    }
}
