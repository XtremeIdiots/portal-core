using Newtonsoft.Json;
using RestSharp;
using XtremeIdiots.Portal.DataLib;

namespace XtremeIdiots.Portal.RepositoryApiClient.GameServersApi;

public class GameServersApiClient : IGameServersApiClient
{
    private readonly string _apimBaseUrl;
    private readonly string _apimSubscriptionKey;

    public GameServersApiClient(string apimBaseUrl, string apimSubscriptionKey)
    {
        _apimBaseUrl = apimBaseUrl;
        _apimSubscriptionKey = apimSubscriptionKey;
    }

    public async Task<List<GameServer>?> GetGameServers(string accessToken)
    {
        var client = new RestClient(_apimBaseUrl);
        var request = new RestRequest("repository/GameServers");

        request.AddHeader("Ocp-Apim-Subscription-Key", _apimSubscriptionKey);
        request.AddHeader("Authorization", $"Bearer {accessToken}");

        var response = await client.ExecuteAsync(request);

        if (response.IsSuccessful && response.Content != null)
            return JsonConvert.DeserializeObject<List<GameServer>>(response.Content);

        if (response.ErrorException != null)
            throw response.ErrorException;

        throw new Exception($"Failed to execute 'repository/GameServers' with '{response.StatusCode}'");
    }
}