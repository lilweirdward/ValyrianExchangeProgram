using Blazored.LocalStorage;
using Blazored.SessionStorage;
using Braavos.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Braavos.Ui.Data
{
    public class UserService : IUserService
    {
        private readonly ILocalStorageService _localStorage;
        private readonly ISessionStorageService _sessionStorage;
        private readonly BraavosClient _client;
        private const string AccountKey = "account";

        public UserService(
            ILocalStorageService localStorageService, 
            ISessionStorageService sessionStorageService,
            BraavosClient client)
        {
            _localStorage = localStorageService;
            _sessionStorage = sessionStorageService;
            _client = client;
        }

        public async Task<AuthorizedUser> GetAccount()
        {
            var localAccount = await _localStorage.GetItemAsync<AuthorizedUser>(AccountKey);
            var sessionAccount = await _sessionStorage.GetItemAsync<AuthorizedUser>(AccountKey);

            if (localAccount != null) return localAccount;
            if (sessionAccount != null) return sessionAccount;

            throw new Exception("No account to get");
        }

        public async Task<bool> IsLoggedIn()
        {
            var existsInLocal = await _localStorage.ContainKeyAsync(AccountKey);
            var existsInSession = (await _sessionStorage.GetItemAsync<AuthorizedUser>(AccountKey)) != null; // Change when ContainKeyAsync is added

            return existsInLocal || existsInSession;
        }

        public async Task LogIn(AuthRequest request, bool rememberMe)
        {
            var account = await _client.AuthorizeAsync(request);

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
