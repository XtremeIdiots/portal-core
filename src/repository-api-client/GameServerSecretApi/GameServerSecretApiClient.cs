using System.Net;
using Azure.Security.KeyVault.Secrets;
using Newtonsoft.Json;
using RestSharp;

namespace XtremeIdiots.Portal.RepositoryApiClient.GameServerSecretApi;

public class GameServerSecretApiClient : IGameServerSecretApiClient
{
    private readonly string _apimBaseUrl;
    private readonly string _apimSubscriptionKey;

    public GameServerSecretApiClient(string apimBaseUrl, string apimSubscriptionKey)
    {
        _apimBaseUrl = apimBaseUrl;
        _apimSubscriptionKey = apimSubscriptionKey;
    }

    public async Task<KeyVaultSecret?> GetGameServerSecret(string accessToken, string id, string secret)
    {
        var client = new RestClient(_apimBaseUrl);
        var request = new RestRequest($"repository/GameServer/{id}/secrets/{secret}");

        request.AddHeader("Ocp-Apim-Subscription-Key", _apimSubscriptionKey);
        request.AddHeader("Authorization", $"Bearer {accessToken}");

        var response = await client.ExecuteAsync(request);

        if (response.IsSuccessful && response.Content != null)
            return JsonConvert.DeserializeObject<KeyVaultSecret>(response.Content);

        if (response.StatusCode == HttpStatusCode.NotFound)
            return null;

        throw new Exception($"Failed to execute 'repository/GameServer/{id}/secrets/{secret}'");
    }

    public async Task UpdateGameServerSecret(string accessToken, string id, string secret, string? secretValue)
    {
        var client = new RestClient(_apimBaseUrl);
        var request = new RestRequest($"repository/GameServer/{id}/secrets/{secret}", Method.Post);

        request.AddHeader("Ocp-Apim-Subscription-Key", _apimSubscriptionKey);
        request.AddHeader("Authorization", $"Bearer {accessToken}");
        request.AddBody(secretValue ?? "", "text/plain");

        await client.ExecuteAsync(request);
    }
}