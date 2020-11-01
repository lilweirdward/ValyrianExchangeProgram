using AutoMapper;
using Braavos.Core.Infrastructure;
using Braavos.Core.Parsers;
using Braavos.Core.Parsers.DataObjects;
using Braavos.Core.Repositories;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

[assembly: FunctionsStartup(typeof(Braavos.Function.Startup))]

namespace Braavos.Function
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddOptions<FunctionOptions>()
                .Configure<IConfiguration>((settings, configuration) => { configuration.GetSection("FunctionOptions").Bind(settings); });
            builder.Services.AddScoped<IBraavosRepository, GoogleSheetsRepository>();
            builder.Services.AddScoped<IDataParser<CnNation>, CnNationParser>();
            builder.Services.AddScoped<ICnDbRepository, CnDbRepository>();

            builder.Services.AddAutoMapper(typeof(Startup));
        }
    }
}
