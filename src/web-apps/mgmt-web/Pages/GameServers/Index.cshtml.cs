using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Identity.Web;
using Newtonsoft.Json;
using RestSharp;
using XtremeIdiots.Portal.DataLib;

namespace XtremeIdiots.Portal.MgmtWeb.Pages.GameServers;

[Authorize(Roles = "ApplicationUser")]
[AuthorizeForScopes(ScopeKeySection = "web-api-repository-scope")]
public class IndexModel : PageModel
{
    private readonly IConfiguration _configuration;
    private readonly ITokenAcquisition _tokenAcquisition;

    public IndexModel(IConfiguration configuration, ITokenAcquisition tokenAcquisition)
    {
        _configuration = configuration;
        _tokenAcquisition = tokenAcquisition;
    }

    public List<GameServer> GameServers { get; set; } = new();

    public async Task OnGet()
    {
        GameServers = await GetGameServers();
    }

    private async Task<List<GameServer>> GetGameServers()
    {
        var client = new RestClient(_configuration["apim-base-url"]);
        var request = new RestRequest("repository/GameServers");

        string[] scopes = {_configuration["web-api-repository-scope"]};
        var accessToken = await _tokenAcquisition.GetAccessTokenForUserAsync(scopes);

        request.AddHeader("Ocp-Apim-Subscription-Key", _configuration["apim-subscription-key"]);
        request.AddHeader("Authorization", $"Bearer {accessToken}");

        var response = await client.ExecuteAsync(request);

        if (response.IsSuccessful)
            return JsonConvert.DeserializeObject<List<GameServer>>(response.Content);

        if (response.ErrorException != null)
            throw response.ErrorException;

        throw new Exception($"Failed to execute 'repository/GameServers' with '{response.StatusCode}'");
    }
}