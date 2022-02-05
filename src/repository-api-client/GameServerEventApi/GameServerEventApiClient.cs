using RestSharp;
using XtremeIdiots.Portal.DataLib;

namespace XtremeIdiots.Portal.RepositoryApiClient.GameServerEventApi;

public class GameServerEventApiClient : BaseApiClient, IGameServerEventApiClient
{
    public GameServerEventApiClient(string apimBaseUrl, string apimSubscriptionKey)
        : base(apimBaseUrl, apimSubscriptionKey)
    {
    }

    public async Task CreateGameServerEvent(string accessToken, string id, GameServerEvent gameServerEvent)
    {
        var request = CreateRequest($"repository/game-servers/{id}/events", Method.Post, accessToken);
        request.AddJsonBody(new List<GameServerEvent> {gameServerEvent});

        await ExecuteAsync(request);
    }
}