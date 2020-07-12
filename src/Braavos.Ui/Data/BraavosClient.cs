using Braavos.Core.Entities;
using Braavos.Core.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
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
    }
}
