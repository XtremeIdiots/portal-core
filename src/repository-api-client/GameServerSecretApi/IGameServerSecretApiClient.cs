using Azure.Security.KeyVault.Secrets;

namespace XtremeIdiots.Portal.RepositoryApiClient.GameServerSecretApi;

public interface IGameServerSecretApiClient
{
    Task<KeyVaultSecret?> GetGameServerSecret(string accessToken, string id, string secret);
    Task UpdateGameServerSecret(string accessToken, string id, string secret, string? secretValue);
}