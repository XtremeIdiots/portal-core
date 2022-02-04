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
        var request = CreateRequest($"repository/GameServer/{id}/event", Method.Post, accessToken);
        request.AddJsonBody(gameServerEvent);

        await ExecuteAsync(request);
    }
}