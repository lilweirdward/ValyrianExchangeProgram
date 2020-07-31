using Braavos.Core.Entities;
using Braavos.Core.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Braavos.Ui.Data
{
    public class BraavosClient
    {
        private readonly HttpClient _client;

        public BraavosClient(HttpClient client) => _client = client;

        public async Task<AuthorizedUser> AuthorizeAsync(AuthRequest authRequest)
        {
            // Try to get a happy-path authorized user
            var response = await _client.PostAsJsonAsync("api/Authenticate", authRequest);
            if (response.IsSuccessStatusCode)
                return await response.Content.ReadFromJsonAsync<AuthorizedUser>();

            // The service should return 401 if a user that doesn't exist was sent
            if (response.IsUnauthorizedStatusCode())
                throw new UnauthorizedAccessException();

            // At this point something unknown happened, so just stringify the response and give it back as an exception
            throw new Exception(await response.Content.ReadAsStringAsync());
        }

        public async Task<Account> GetAccountAsync(AuthorizedUser authorizedUser)
        {
            // Try to get a happy-path account
            var response = await _client.PostAsJsonAsync("api/GetAccountDetails", authorizedUser);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();

                return JsonSerializer.Deserialize<Account>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }

            // The service should return 404 if an account was not found for the authorized user
            if (response.IsNotFoundStatusCode())
                throw new KeyNotFoundException();

            // At this point something unknown happened, so just stringify the response and give it back as an exception
            throw new Exception(await response.Content.ReadAsStringAsync());
        }
    }
}
