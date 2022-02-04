using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using XtremeIdiots.Portal.FuncHelpers.Providers;
using XtremeIdiots.Portal.IngestFunc;
using XtremeIdiots.Portal.RepositoryApiClient.GameServerApi;
using XtremeIdiots.Portal.RepositoryApiClient.GameServerEventApi;
using XtremeIdiots.Portal.RepositoryApiClient.PlayersApi;

[assembly: FunctionsStartup(typeof(Startup))]

namespace XtremeIdiots.Portal.IngestFunc;

public class Startup : FunctionsStartup
{
    public override void Configure(IFunctionsHostBuilder builder)
    {
        var config = builder.GetContext().Configuration;

        builder.Services.AddSingleton<IRepositoryTokenProvider, RepositoryTokenProvider>();
        builder.Services.AddSingleton<IPlayersApiClient, PlayersApiClient>(_ =>
            new PlayersApiClient(config["apim-base-url"], config["apim-subscription-key"]));
        builder.Services.AddSingleton<IGameServerApiClient, GameServerApiClient>(_ =>
            new GameServerApiClient(config["apim-base-url"], config["apim-subscription-key"]));
        builder.Services.AddSingleton<IGameServerEventApiClient, GameServerEventApiClient>(_ =>
            new GameServerEventApiClient(config["apim-base-url"], config["apim-subscription-key"]));
        builder.Services.AddLogging();
    }
}