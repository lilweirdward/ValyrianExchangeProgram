using Blazored.LocalStorage;
using Blazored.SessionStorage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Braavos.Ui.Data
{
    public class LoginAccountService : ILoginAccountService
    {
        private readonly ILocalStorageService _localStorage;
        private readonly ISessionStorageService _sessionStorage;
        private const string AccountKey = "account";

        public LoginAccountService(ILocalStorageService localStorageService, ISessionStorageService sessionStorageService)
        {
            _localStorage = localStorageService;
            _sessionStorage = sessionStorageService;
        }

        public async Task<LoginAccount> GetAccount()
        {
            var localAccount = await _localStorage.GetItemAsync<LoginAccount>(AccountKey);
            var sessionAccount = await _sessionStorage.GetItemAsync<LoginAccount>(AccountKey);

            if (localAccount != null) return localAccount;
            if (sessionAccount != null) return sessionAccount;

            throw new Exception("No account to get");
        }

        public async Task<bool> IsLoggedIn()
        {
            var existsInLocal = await _localStorage.ContainKeyAsync(AccountKey);
            var existsInSession = (await _sessionStorage.GetItemAsync<LoginAccount>(AccountKey)) != null; // Change when ContainKeyAsync is added

            return existsInLocal || existsInSession;
        }

        public async Task LogIn(LoginAccount account, bool rememberMe)
        {
            if (rememberMe)
                await _localStorage.SetItemAsync(AccountKey, account);
            else
                await _sessionStorage.SetItemAsync(AccountKey, account);
        }

        public async Task LogOut()
        {
            // Blow both away just in case
            await _localStorage.RemoveItemAsync(AccountKey);
            await _sessionStorage.RemoveItemAsync(AccountKey);
        }
    }
}
