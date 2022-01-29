using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Identity.Web;
using RestSharp;
using XtremeIdiots.Portal.DataLib;
using XtremeIdiots.Portal.MgmtWeb.ViewModels;

namespace XtremeIdiots.Portal.MgmtWeb.Pages.GameServers;

[Authorize(Roles = "ApplicationUser")]
[AuthorizeForScopes(ScopeKeySection = "web-api-repository-scope")]
public class CreateModel : PageModel
{
    private readonly IConfiguration _configuration;
    private readonly ITokenAcquisition _tokenAcquisition;

    public CreateModel(IConfiguration configuration, ITokenAcquisition tokenAcquisition)
    {
        _configuration = configuration;
        _tokenAcquisition = tokenAcquisition;
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

        await CreateGameServer(gameServer);

        return RedirectToPage("./Index");
    }

    private async Task CreateGameServer(GameServer gameServer)
    {
        var client = new RestClient(_configuration["apim-base-url"]);
        var request = new RestRequest("repository/GameServerViewModel", Method.Post);

        string[] scopes = {_configuration["web-api-repository-scope"]};
        var accessToken = await _tokenAcquisition.GetAccessTokenForUserAsync(scopes);

        request.AddHeader("Ocp-Apim-Subscription-Key", _configuration["apim-subscription-key"]);
        request.AddHeader("Authorization", $"Bearer {accessToken}");
        request.AddJsonBody(gameServer);

        await client.ExecuteAsync(request);
    }
}