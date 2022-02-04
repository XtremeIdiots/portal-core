using System.Net;
using Newtonsoft.Json;
using RestSharp;
using XtremeIdiots.Portal.DataLib;

namespace XtremeIdiots.Portal.RepositoryApiClient.PlayersApi;

public class PlayersApiClient : BaseApiClient, IPlayersApiClient
{
    public PlayersApiClient(string apimBaseUrl, string apimSubscriptionKey)
        : base(apimBaseUrl, apimSubscriptionKey)
    {
    }

    public async Task<Player?> GetPlayer(string accessToken, string gameType, string guid)
    {
        var request = CreateRequest("repository/Player", Method.Get, accessToken);
        request.AddParameter(new QueryParameter("gameType", gameType));
        request.AddParameter(new QueryParameter("guid", guid));

        var response = await ExecuteAsync(request);

        if (response.IsSuccessful && response.Content != null)
            return JsonConvert.DeserializeObject<Player>(response.Content);
        if (response.StatusCode == HttpStatusCode.NotFound)
            return null;
        throw new Exception("Failed to execute 'repository/Player'");
    }

    public async Task CreatePlayer(string accessToken, Player player)
    {
        var request = CreateRequest("repository/Player", Method.Post, accessToken);
        request.AddJsonBody(player);

        await ExecuteAsync(request);
    }

    public async Task UpdatePlayer(string accessToken, Player player)
    {
        var request = CreateRequest("repository/Player", Method.Patch, accessToken);
        request.AddJsonBody(player);

        await ExecuteAsync(request);
    }

    public async Task CreateChatMessage(string accessToken, ChatMessage chatMessage)
    {
        var request = CreateRequest("repository/ChatMessage", Method.Post, accessToken);
        request.AddJsonBody(chatMessage);

        await ExecuteAsync(request);
    }
}