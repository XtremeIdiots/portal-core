using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using XtremeIdiots.Portal.FuncHelpers.Providers;
using XtremeIdiots.Portal.IngestFunc;
using XtremeIdiots.Portal.RepositoryApiClient.ChatMessagesApi;
using XtremeIdiots.Portal.RepositoryApiClient.GameServersApi;
using XtremeIdiots.Portal.RepositoryApiClient.GameServersEventsApi;
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
        builder.Services.AddSingleton<IGameServersApiClient, GameServersApiClient>(_ =>
            new GameServersApiClient(config["apim-base-url"], config["apim-subscription-key"]));
        builder.Services.AddSingleton<IGameServersEventsApiClient, GameServersEventsApiClient>(_ =>
            new GameServersEventsApiClient(config["apim-base-url"], config["apim-subscription-key"]));
        builder.Services.AddSingleton<IChatMessagesApiClient, ChatMessagesApiClient>(_ =>
            new ChatMessagesApiClient(config["apim-base-url"], config["apim-subscription-key"]));
        builder.Services.AddLogging();
    }
}