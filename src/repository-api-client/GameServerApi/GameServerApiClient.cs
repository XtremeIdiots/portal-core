using System.Net;
using Newtonsoft.Json;
using RestSharp;
using XtremeIdiots.Portal.DataLib;

namespace XtremeIdiots.Portal.RepositoryApiClient.GameServerApi;

public class GameServerApiClient : IGameServerApiClient
{
    private readonly string _apimBaseUrl;
    private readonly string _apimSubscriptionKey;

    public GameServerApiClient(string apimBaseUrl, string apimSubscriptionKey)
    {
        _apimBaseUrl = apimBaseUrl;
        _apimSubscriptionKey = apimSubscriptionKey;
    }

    public async Task<GameServer?> GetGameServer(string accessToken, string id)
    {
        var client = new RestClient(_apimBaseUrl);
        var request = new RestRequest("repository/GameServer");

        request.AddHeader("Ocp-Apim-Subscription-Key", _apimSubscriptionKey);
        request.AddHeader("Authorization", $"Bearer {accessToken}");
        request.AddParameter(new QueryParameter("id", id));

        var response = await client.ExecuteAsync(request);

        if (response.IsSuccessful && response.Content != null)
            return JsonConvert.DeserializeObject<GameServer>(response.Content);
        if (response.StatusCode == HttpStatusCode.NotFound)
            return null;
        throw new Exception("Failed to execute 'repository/GameServer'");
    }

    public async Task CreateGameServer(string accessToken, GameServer gameServer)
    {
        var client = new RestClient(_apimBaseUrl);
        var request = new RestRequest("repository/GameServer", Method.Post);

        request.AddHeader("Ocp-Apim-Subscription-Key", _apimSubscriptionKey);
        request.AddHeader("Authorization", $"Bearer {accessToken}");
        request.AddJsonBody(gameServer);

        await client.ExecuteAsync(request);
    }

    public async Task UpdateGameServer(string accessToken, GameServer gameServer)
    {
        var client = new RestClient(_apimBaseUrl);
        var request = new RestRequest("repository/GameServer", Method.Patch);

        request.AddHeader("Ocp-Apim-Subscription-Key", _apimSubscriptionKey);
        request.AddHeader("Authorization", $"Bearer {accessToken}");
        request.AddJsonBody(gameServer);

        await client.ExecuteAsync(request);
    }
}