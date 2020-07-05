using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using Braavos.Entities;

namespace Braavos.Function
{
    public static class EntryPoint
    {
        [FunctionName(nameof(Authenticate))]
        public static async Task<IActionResult> Authenticate(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            var content = await new StreamReader(req.Body).ReadToEndAsync();

            try
            {
                var authRequest = JsonSerializer.Deserialize<AuthRequest>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                // Do stuff to actually validate the auth request

                return new OkObjectResult(authRequest);
            }
            catch (Exception e)
            {
                log.LogError(e, "Unknown exception caught while processing an Authenticate request.");

                return new BadRequestObjectResult("Something bad happened :( ");
            }
        }
    }
}
