using System.Net;
using Newtonsoft.Json;
using RestSharp;
using XtremeIdiots.Portal.DataLib;

namespace XtremeIdiots.Portal.RepositoryApiClient.PlayersApi;

public class PlayersApiClient : IPlayersApiClient
{
    private readonly string _apimBaseUrl;
    private readonly string _apimSubscriptionKey;

    public PlayersApiClient(string apimBaseUrl, string apimSubscriptionKey)
    {
        _apimBaseUrl = apimBaseUrl;
        _apimSubscriptionKey = apimSubscriptionKey;
    }

    public async Task<Player?> GetPlayer(string accessToken, string gameType, string guid)
    {
        var client = new RestClient(_apimBaseUrl);
        var request = new RestRequest("repository/Player");

        request.AddHeader("Ocp-Apim-Subscription-Key", _apimSubscriptionKey);
        request.AddHeader("Authorization", $"Bearer {accessToken}");
        request.AddParameter(new QueryParameter("gameType", gameType));
        request.AddParameter(new QueryParameter("guid", guid));

        var response = await client.ExecuteAsync(request);

        if (response.IsSuccessful && response.Content != null)
            return JsonConvert.DeserializeObject<Player>(response.Content);
        if (response.StatusCode == HttpStatusCode.NotFound)
            return null;
        throw new Exception("Failed to execute 'repository/Player'");
    }

    public async Task CreatePlayer(string accessToken, Player player)
    {
        var client = new RestClient(_apimBaseUrl);
        var request = new RestRequest("repository/Player", Method.Post);

        request.AddHeader("Ocp-Apim-Subscription-Key", _apimSubscriptionKey);
        request.AddHeader("Authorization", $"Bearer {accessToken}");
        request.AddJsonBody(player);

        await client.ExecuteAsync(request);
    }

    public async Task UpdatePlayer(string accessToken, Player player)
    {
        var client = new RestClient(_apimBaseUrl);
        var request = new RestRequest("repository/Player", Method.Patch);

        request.AddHeader("Ocp-Apim-Subscription-Key", _apimSubscriptionKey);
        request.AddHeader("Authorization", $"Bearer {accessToken}");
        request.AddJsonBody(player);

        await client.ExecuteAsync(request);
    }

    public async Task CreateChatMessage(string accessToken, ChatMessage chatMessage)
    {
        var client = new RestClient(_apimBaseUrl);
        var request = new RestRequest("repository/ChatMessage", Method.Post);

        request.AddHeader("Ocp-Apim-Subscription-Key", _apimSubscriptionKey);
        request.AddHeader("Authorization", $"Bearer {accessToken}");
        request.AddJsonBody(chatMessage);

        await client.ExecuteAsync(request);
    }
}