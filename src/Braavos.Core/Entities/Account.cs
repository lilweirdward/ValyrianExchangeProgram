using System;
using System.Collections.Generic;
using System.Text;

namespace Braavos.Core.Entities
{
    public class Account
    {
        public string RulerName { get; set; }
        public string NationName { get; set; }
        public Role Role { get; set; }
        public string Alliance { get; set; }
        public Balance Balance { get; set; }
        public int? AvailableSlots { get; set; }
    }
}
