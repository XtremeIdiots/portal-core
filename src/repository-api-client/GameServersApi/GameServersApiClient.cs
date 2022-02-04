using Newtonsoft.Json;
using RestSharp;
using XtremeIdiots.Portal.DataLib;

namespace XtremeIdiots.Portal.RepositoryApiClient.GameServersApi;

public class GameServersApiClient : BaseApiClient, IGameServersApiClient
{
    public GameServersApiClient(string apimBaseUrl, string apimSubscriptionKey)
        : base(apimBaseUrl, apimSubscriptionKey)
    {
    }

    public async Task<List<GameServer>?> GetGameServers(string accessToken)
    {
        var request = CreateRequest("repository/GameServers", Method.Get, accessToken);
        var response = await ExecuteAsync(request);

        if (response.IsSuccessful && response.Content != null)
            return JsonConvert.DeserializeObject<List<GameServer>>(response.Content);

        if (response.ErrorException != null)
            throw response.ErrorException;

        throw new Exception($"Failed to execute 'repository/GameServers' with '{response.StatusCode}'");
    }
}