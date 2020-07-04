using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Braavos.Ui.Data
{
    public class LoginAccount
    {
        public int NationId { get; set; }
        public string RulerName { get; set; }
        public string UniqueCode { get; set; }

        public LoginAccount(int nationId, string uniqueCode) => (NationId, UniqueCode) = (nationId, uniqueCode);

        public LoginAccount(string rulerName, string uniqueCode) => (RulerName, UniqueCode) = (rulerName, uniqueCode);
    }
}
