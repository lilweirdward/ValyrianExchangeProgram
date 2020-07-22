using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using Braavos.Core.Entities;
using Braavos.Core.Repositories;

namespace Braavos.Function
{
    public class EntryPoint
    {
        private readonly IBraavosRepository _repository;

        public EntryPoint(IBraavosRepository repository) => _repository = repository;

        [FunctionName(nameof(Authenticate))]
        public async Task<IActionResult> Authenticate(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            var content = await new StreamReader(req.Body).ReadToEndAsync();

            try
            {
                var authRequest = JsonSerializer.Deserialize<AuthRequest>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                var authorizedUser = await _repository.Authorize(authRequest);

                // Authorize will return null if the request is unauthorized
                if (authorizedUser is null) return new UnauthorizedResult();

                return new OkObjectResult(authorizedUser);
            }
            catch (Exception e)
            {
                log.LogError(e, "Unknown exception caught while processing an Authenticate request.");

                return new BadRequestObjectResult("Something bad happened :( ");
            }
        }

        [FunctionName(nameof(GetAccountDetails))]
        public async Task<IActionResult> GetAccountDetails(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            var content = await new StreamReader(req.Body).ReadToEndAsync();

            try
            {
                var authorizedUser = JsonSerializer.Deserialize<AuthorizedUser>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                var account = await _repository.GetAccountDetails(authorizedUser);

                // Repository will return null if no account exists
                if (account is null) return new NotFoundResult();

                return new OkObjectResult(account);
            }
            catch (Exception e)
            {
                log.LogError(e, "Unknown exception caught while processing a request to fetch Account details");

                return new BadRequestObjectResult("Something bad happened :( ");
            }
        }
    }
}
