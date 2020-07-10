using System;
using System.Collections.Generic;
using System.Text;

namespace Braavos.Core.Entities
{
    public class AuthorizedUser
    {
        public int NationId { get; set; }
        public string RulerName { get; set; }
        public string UniqueCode { get; set; }
    }
}
