using System;
using System.Threading.Tasks;
using Azure.Core;
using Azure.Identity;
using Microsoft.Extensions.Logging;

namespace XtremeIdiots.Portal.IngestFunc.Providers;

internal class RepositoryTokenProvider : IRepositoryTokenProvider
{
    public RepositoryTokenProvider(ILogger<RepositoryTokenProvider> log)
    {
        Log = log;
    }

    private ILogger Log { get; }

    private string WebApiPortalApplicationAudience =>
        Environment.GetEnvironmentVariable("webapi-portal-application-audience");

    public async Task<string> GetRepositoryAccessToken()
    {
        var tokenCredential = new ManagedIdentityCredential();

        AccessToken accessToken;
        try
        {
            accessToken = await tokenCredential.GetTokenAsync(
                new TokenRequestContext(new[] {$"{WebApiPortalApplicationAudience}/.default"}));

            Log.LogInformation($"AccessToken: {accessToken.Token}");
        }
        catch (Exception ex)
        {
            Log.LogError(ex, "Failed to get managed identity token");
            throw;
        }

        return accessToken.Token;
    }
}