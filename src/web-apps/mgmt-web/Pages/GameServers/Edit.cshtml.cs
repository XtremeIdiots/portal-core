using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Identity.Web;
using XtremeIdiots.Portal.MgmtWeb.ViewModels;
using XtremeIdiots.Portal.RepositoryApiClient.GameServerApi;
using XtremeIdiots.Portal.RepositoryApiClient.GameServerSecretApi;

namespace XtremeIdiots.Portal.MgmtWeb.Pages.GameServers;

[Authorize(Roles = "ApplicationUser")]
[AuthorizeForScopes(ScopeKeySection = "web-api-repository-scope")]
public class EditModel : PageModel
{
    private readonly IConfiguration _configuration;
    private readonly IGameServerApiClient _gameServerApiClient;
    private readonly IGameServerSecretApiClient _gameServerSecretApiClient;
    private readonly ITokenAcquisition _tokenAcquisition;

    public EditModel(
        IConfiguration configuration,
        ITokenAcquisition tokenAcquisition,
        IGameServerApiClient gameServerApiClient,
        IGameServerSecretApiClient gameServerSecretApiClient)
    {
        _configuration = configuration;
        _tokenAcquisition = tokenAcquisition;
        _gameServerApiClient = gameServerApiClient;
        _gameServerSecretApiClient = gameServerSecretApiClient;
    }

    [BindProperty] public GameServerViewModel GameServerViewModel { get; set; }

    public async Task<IActionResult> OnGet(string? id)
    {
        if (string.IsNullOrWhiteSpace(id)) return NotFound();

        string[] scopes = {_configuration["web-api-repository-scope"]};
        var accessToken = await _tokenAcquisition.GetAccessTokenForUserAsync(scopes);

        var gameServer = await _gameServerApiClient.GetGameServer(accessToken, id);

        if (gameServer == null)
            return NotFound();

        var rconPasswordSecret = await _gameServerSecretApiClient.GetGameServerSecret(accessToken, id, "rconpassword");
        var ftpUsernameSecret = await _gameServerSecretApiClient.GetGameServerSecret(accessToken, id, "ftpusername");
        var ftpPasswordSecret = await _gameServerSecretApiClient.GetGameServerSecret(accessToken, id, "ftppassword");

        GameServerViewModel = new GameServerViewModel
        {
            Id = gameServer.Id,
            Title = gameServer.Title,
            GameType = gameServer.GameType,
            IpAddress = gameServer.IpAddress,
            QueryPort = gameServer.QueryPort,
            RconPassword = rconPasswordSecret?.Value,
            FtpUsername = ftpUsernameSecret?.Value,
            FtpPassword = ftpPasswordSecret?.Value
        };

        return Page();
    }

    public async Task<IActionResult> OnPostAsync(string id)
    {
        if (!ModelState.IsValid) return Page();

        string[] scopes = {_configuration["web-api-repository-scope"]};
        var accessToken = await _tokenAcquisition.GetAccessTokenForUserAsync(scopes);

        var gameServer = await _gameServerApiClient.GetGameServer(accessToken, id);

        if (gameServer == null)
            return NotFound();

        gameServer.Title = GameServerViewModel.Title;
        gameServer.GameType = GameServerViewModel.GameType;
        gameServer.IpAddress = GameServerViewModel.IpAddress;
        gameServer.QueryPort = GameServerViewModel.QueryPort;

        await _gameServerApiClient.UpdateGameServer(accessToken, gameServer);
        await _gameServerSecretApiClient.UpdateGameServerSecret(accessToken, id, "rconpassword",
            GameServerViewModel.RconPassword);
        await _gameServerSecretApiClient.UpdateGameServerSecret(accessToken, id, "ftpusername",
            GameServerViewModel.FtpUsername);
        await _gameServerSecretApiClient.UpdateGameServerSecret(accessToken, id, "ftppassword",
            GameServerViewModel.FtpPassword);

        return RedirectToPage("Index");
    }
}