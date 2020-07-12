using Braavos.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Braavos.Ui.Data
{
    public interface IUserService
    {
        Task LogIn(AuthRequest account, bool rememberMe);
        Task LogOut();
        Task<bool> IsLoggedIn();
        Task<AuthorizedUser> GetAccount();
    }
}
