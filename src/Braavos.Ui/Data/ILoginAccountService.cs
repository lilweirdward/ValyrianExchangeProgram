using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Braavos.Ui.Data
{
    public interface ILoginAccountService
    {
        Task LogIn(LoginAccount account, bool rememberMe);
        Task LogOut();
        Task<bool> IsLoggedIn();
        Task<LoginAccount> GetAccount();
    }
}
