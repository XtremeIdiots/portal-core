using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using XtremeIdiots.Portal.FuncHelpers.Providers;
using XtremeIdiots.Portal.RepositoryApiClient.DataMaintenanceApi;
using XtremeIdiots.Portal.RepositoryFunc;

[assembly: FunctionsStartup(typeof(Startup))]

namespace XtremeIdiots.Portal.RepositoryFunc;

public class Startup : FunctionsStartup
{
    public override void Configure(IFunctionsHostBuilder builder)
    {
        var config = builder.GetContext().Configuration;

        builder.Services.AddSingleton<IRepositoryTokenProvider, RepositoryTokenProvider>();
        builder.Services.AddSingleton<IDataMaintenanceApiClient, DataMaintenanceApiClient>(_ =>
            new DataMaintenanceApiClient(config["apim-base-url"], config["apim-subscription-key"]));
        builder.Services.AddLogging();
    }
}