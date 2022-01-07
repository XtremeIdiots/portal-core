using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using XtremeIdiots.Portal.IngestFunc;
using XtremeIdiots.Portal.IngestFunc.Providers;

[assembly: FunctionsStartup(typeof(Startup))]

namespace XtremeIdiots.Portal.IngestFunc;

public class Startup : FunctionsStartup
{
    public override void Configure(IFunctionsHostBuilder builder)
    {
        builder.Services.AddSingleton<IRepositoryTokenProvider, RepositoryTokenProvider>();
        builder.Services.AddLogging();
    }
}