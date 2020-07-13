using System.Threading.Tasks;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Blazored.LocalStorage;
using Blazored.SessionStorage;
using Braavos.Ui.Data;
using System;

namespace Braavos.Ui
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("app");

            builder.Services.AddBlazoredLocalStorage();
            builder.Services.AddBlazoredSessionStorage();
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddHttpClient<BraavosClient>(client => client.BaseAddress = new Uri(builder.Configuration["BraavosFunctionUri"]));

            await builder.Build().RunAsync();
        }
    }
}
