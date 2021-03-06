using Microsoft.Extensions.Options;
using RestSharp;
using XtremeIdiots.Portal.DataLib;

namespace XtremeIdiots.Portal.RepositoryApiClient.GameServersEventsApi;

public class GameServersEventsApiClient : BaseApiClient, IGameServersEventsApiClient
{
    public GameServersEventsApiClient(IOptions<RepositoryApiClientOptions> options) : base(options)
    {
    }

    public async Task CreateGameServerEvent(string accessToken, string id, GameServerEvent gameServerEvent)
    {
        var request = CreateRequest($"repository/game-servers/{id}/events", Method.Post, accessToken);
        request.AddJsonBody(new List<GameServerEvent> {gameServerEvent});

        await ExecuteAsync(request);
    }
}