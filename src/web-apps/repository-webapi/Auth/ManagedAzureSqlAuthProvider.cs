using Azure.Core;
using Azure.Identity;
using Microsoft.Data.SqlClient;

namespace XtremeIdiots.Portal.RepositoryWebApi.Auth;

public class ManagedAzureSqlAuthProvider : SqlAuthenticationProvider
{
    private readonly ILogger<ManagedAzureSqlAuthProvider> _log;

    public ManagedAzureSqlAuthProvider(ILogger<ManagedAzureSqlAuthProvider> log)
    {
        _log = log;
    }

    private static readonly string[] AzureSqlScopes =
    {
        "https://database.windows.net//.default"
    };

    private static readonly TokenCredential Credential = new DefaultAzureCredential();

    public override async Task<SqlAuthenticationToken> AcquireTokenAsync(SqlAuthenticationParameters parameters)
    {
        var tokenRequestContext = new TokenRequestContext(AzureSqlScopes);
        var tokenResult = await Credential.GetTokenAsync(tokenRequestContext, default);
        _log.LogInformation($"SQL Token: {tokenResult.Token}");
        return new SqlAuthenticationToken(tokenResult.Token, tokenResult.ExpiresOn);
    }

    public override bool IsSupported(SqlAuthenticationMethod authenticationMethod)
    {
        return authenticationMethod.Equals(SqlAuthenticationMethod.ActiveDirectoryDeviceCodeFlow);
    }
}