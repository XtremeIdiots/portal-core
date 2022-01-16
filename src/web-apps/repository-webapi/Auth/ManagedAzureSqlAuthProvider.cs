using Azure.Core;
using Azure.Identity;
using Microsoft.Data.SqlClient;

namespace XtremeIdiots.Portal.RepositoryWebApi.Auth;

public class ManagedAzureSqlAuthProvider : SqlAuthenticationProvider
{
    private static readonly string[] AzureSqlScopes =
    {
        "https://database.windows.net//.default"
    };

    private static readonly TokenCredential Credential = new DefaultAzureCredential();

    public override async Task<SqlAuthenticationToken> AcquireTokenAsync(SqlAuthenticationParameters parameters)
    {
        var tokenRequestContext = new TokenRequestContext(AzureSqlScopes);
        var tokenResult = await Credential.GetTokenAsync(tokenRequestContext, default);
        return new SqlAuthenticationToken(tokenResult.Token, tokenResult.ExpiresOn);
    }

    public override bool IsSupported(SqlAuthenticationMethod authenticationMethod)
    {
        return authenticationMethod.Equals(SqlAuthenticationMethod.ActiveDirectoryDeviceCodeFlow);
    }
}