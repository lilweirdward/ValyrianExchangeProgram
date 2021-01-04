using AutoMapper;
using Braavos.Core.Grabbers;
using Braavos.Core.Infrastructure;
using Braavos.Core.Parsers;
using Braavos.Core.Parsers.DataObjects;
using Braavos.Core.Repositories;
using Braavos.Core.Repositories.DbContexts;
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
            builder.Services.AddScoped<ICybernationsDbContext, CybernationsDbContext>();
            builder.Services.AddScoped<IBraavosRepository, GoogleSheetsRepository>();
            builder.Services.AddScoped<IDataParser, CnFileParser>();
            builder.Services.AddScoped<ICnDbRepository, CnDbRepository>();
            builder.Services.AddScoped<IDataGrabber, CnFileGrabber>();

            builder.Services.AddAutoMapper(typeof(Startup));
        }
    }
}
