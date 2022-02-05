using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Identity.Web;
using XtremeIdiots.Portal.DataLib;
using XtremeIdiots.Portal.MgmtWeb.ViewModels;
using XtremeIdiots.Portal.RepositoryApiClient;

namespace XtremeIdiots.Portal.MgmtWeb.Pages.GameServers;

[Authorize(Roles = "ApplicationUser")]
[AuthorizeForScopes(ScopeKeySection = "web-api-repository-scope")]
public class CreateModel : PageModel
{
    private readonly IConfiguration _configuration;
    private readonly IRepositoryApiClient _repositoryApiClient;
    private readonly ITokenAcquisition _tokenAcquisition;

    public CreateModel(
        IConfiguration configuration,
        ITokenAcquisition tokenAcquisition,
        IRepositoryApiClient repositoryApiClient)
    {
        _configuration = configuration;
        _tokenAcquisition = tokenAcquisition;
        _repositoryApiClient = repositoryApiClient;
    }

    [BindProperty] public GameServerViewModel GameServerViewModel { get; set; }

    public void OnGet()
    {
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid) return Page();

        var gameServer = new GameServer
        {
            Id = GameServerViewModel.Id,
            Title = GameServerViewModel.Title,
            GameType = GameServerViewModel.GameType,
            IpAddress = GameServerViewModel.IpAddress,
            QueryPort = GameServerViewModel.QueryPort
        };

        string[] scopes = {_configuration["web-api-repository-scope"]};
        var accessToken = await _tokenAcquisition.GetAccessTokenForUserAsync(scopes);

        await _repositoryApiClient.GameServers.CreateGameServer(accessToken, gameServer);

        return RedirectToPage("./Index");
    }
}