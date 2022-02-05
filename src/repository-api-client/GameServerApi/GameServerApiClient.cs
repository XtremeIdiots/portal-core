using System.Net;
using Newtonsoft.Json;
using RestSharp;
using XtremeIdiots.Portal.DataLib;

namespace XtremeIdiots.Portal.RepositoryApiClient.GameServerApi;

public class GameServerApiClient : BaseApiClient, IGameServerApiClient
{
    public GameServerApiClient(string apimBaseUrl, string apimSubscriptionKey)
        : base(apimBaseUrl, apimSubscriptionKey)
    {
    }

    public async Task<GameServer?> GetGameServer(string accessToken, string id)
    {
        var request = CreateRequest($"repository/game-servers/{id}", Method.Get, accessToken);

        var response = await ExecuteAsync(request);

        if (response.IsSuccessful && response.Content != null)
            return JsonConvert.DeserializeObject<GameServer>(response.Content);
        if (response.StatusCode == HttpStatusCode.NotFound)
            return null;
        throw new Exception($"Failed to execute 'repository/game-servers/{id}'");
    }

    public async Task CreateGameServer(string accessToken, GameServer gameServer)
    {
        var request = CreateRequest("repository/game-servers", Method.Post, accessToken);
        request.AddJsonBody(new List<GameServer> {gameServer});

        await ExecuteAsync(request);
    }

    public async Task UpdateGameServer(string accessToken, GameServer gameServer)
    {
        var request = CreateRequest($"repository/game-servers/{gameServer.Id}", Method.Patch, accessToken);
        request.AddJsonBody(gameServer);

        await ExecuteAsync(request);
    }
}