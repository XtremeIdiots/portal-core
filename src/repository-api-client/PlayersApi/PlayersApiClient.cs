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

    public async Task<Player?> GetPlayer(string accessToken, Guid id)
    {
        var request = CreateRequest($"repository/players/{id}", Method.Get, accessToken);
        var response = await ExecuteAsync(request);

        if (response.IsSuccessful && response.Content != null)
            return JsonConvert.DeserializeObject<Player>(response.Content);
        if (response.StatusCode == HttpStatusCode.NotFound)
            return null;
        throw new Exception("Failed to execute 'repository/Player'");
    }

    public async Task<Player?> GetPlayerByGameType(string accessToken, string gameType, string guid)
    {
        var request = CreateRequest($"repository/players/by-game-type/{gameType}/{guid}", Method.Get, accessToken);

        var response = await ExecuteAsync(request);

        if (response.IsSuccessful && response.Content != null)
            return JsonConvert.DeserializeObject<Player>(response.Content);
        if (response.StatusCode == HttpStatusCode.NotFound)
            return null;
        throw new Exception($"Failed to execute 'repository/players/by-game-type/{gameType}/{guid}'");
    }

    public async Task CreatePlayer(string accessToken, Player player)
    {
        var request = CreateRequest("repository/players", Method.Post, accessToken);
        request.AddJsonBody(new List<Player> {player});

        await ExecuteAsync(request);
    }

    public async Task UpdatePlayer(string accessToken, Player player)
    {
        var request = CreateRequest($"repository/players/{player.Id}", Method.Patch, accessToken);
        request.AddJsonBody(player);

        await ExecuteAsync(request);
    }

    public async Task CreateChatMessage(string accessToken, ChatMessage chatMessage)
    {
        var request = CreateRequest("repository/chat-messages", Method.Post, accessToken);
        request.AddJsonBody(new List<ChatMessage> {chatMessage});

        await ExecuteAsync(request);
    }
}