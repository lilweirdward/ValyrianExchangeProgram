using System;
using System.Collections.Generic;
using System.Text;

namespace Braavos.Core.Entities
{
    public class ValidAccount : Account
    {
        public bool ShouldBeSendingAid { get; set; }

        public ValidAccount() { }

        public ValidAccount(Account account) : base(account) { }
    }
}
