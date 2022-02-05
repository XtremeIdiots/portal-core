using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Identity.Web;
using XtremeIdiots.Portal.DataLib;
using XtremeIdiots.Portal.RepositoryApiClient;

namespace XtremeIdiots.Portal.MgmtWeb.Pages.GameServers;

[Authorize(Roles = "ApplicationUser")]
[AuthorizeForScopes(ScopeKeySection = "web-api-repository-scope")]
public class IndexModel : PageModel
{
    private readonly IConfiguration _configuration;
    private readonly IRepositoryApiClient _repositoryApiClient;
    private readonly ITokenAcquisition _tokenAcquisition;

    public IndexModel(
        IConfiguration configuration,
        ITokenAcquisition tokenAcquisition,
        IRepositoryApiClient repositoryApiClient)
    {
        _configuration = configuration;
        _tokenAcquisition = tokenAcquisition;
        _repositoryApiClient = repositoryApiClient;
    }

    public List<GameServer> GameServers { get; set; } = new();

    public async Task OnGet()
    {
        string[] scopes = {_configuration["web-api-repository-scope"]};
        var accessToken = await _tokenAcquisition.GetAccessTokenForUserAsync(scopes);

        GameServers = await _repositoryApiClient.GameServers.GetGameServers(accessToken) ?? new List<GameServer>();
    }
}