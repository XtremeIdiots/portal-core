using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Identity.Web;
using XtremeIdiots.Portal.CommonLib.Constants;
using XtremeIdiots.Portal.MgmtWeb.ViewModels;
using XtremeIdiots.Portal.RepositoryApiClient;

namespace XtremeIdiots.Portal.MgmtWeb.Pages.GameServers;

[Authorize(Roles = "ApplicationUser")]
[AuthorizeForScopes(ScopeKeySection = "web-api-repository-scope")]
public class EditModel : PageModel
{
    private readonly IConfiguration _configuration;
    private readonly IRepositoryApiClient _repositoryApiClient;
    private readonly ITokenAcquisition _tokenAcquisition;

    public EditModel(
        IConfiguration configuration,
        ITokenAcquisition tokenAcquisition,
        IRepositoryApiClient repositoryApiClient)
    {
        _configuration = configuration;
        _tokenAcquisition = tokenAcquisition;
        _repositoryApiClient = repositoryApiClient;
    }

    [BindProperty] public GameServerViewModel GameServerViewModel { get; set; }

    public async Task<IActionResult> OnGet(string? id)
    {
        if (string.IsNullOrWhiteSpace(id)) return NotFound();

        string[] scopes = {_configuration["web-api-repository-scope"]};
        var accessToken = await _tokenAcquisition.GetAccessTokenForUserAsync(scopes);

        var gameServer = await _repositoryApiClient.GameServers.GetGameServer(accessToken, id);

        if (gameServer == null)
            return NotFound();

        var rconPasswordSecret =
            await _repositoryApiClient.GameServersSecrets.GetGameServerSecret(accessToken, id,
                SecretNames.RconPassword);
        var ftpUsernameSecret =
            await _repositoryApiClient.GameServersSecrets.GetGameServerSecret(accessToken, id, SecretNames.FtpUsername);
        var ftpPasswordSecret =
            await _repositoryApiClient.GameServersSecrets.GetGameServerSecret(accessToken, id, SecretNames.FtpPassword);

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

        var gameServer = await _repositoryApiClient.GameServers.GetGameServer(accessToken, id);

        if (gameServer == null)
            return NotFound();

        gameServer.Title = GameServerViewModel.Title;
        gameServer.GameType = GameServerViewModel.GameType;
        gameServer.IpAddress = GameServerViewModel.IpAddress;
        gameServer.QueryPort = GameServerViewModel.QueryPort;

        await _repositoryApiClient.GameServers.UpdateGameServer(accessToken, gameServer);
        await _repositoryApiClient.GameServersSecrets.UpdateGameServerSecret(accessToken, id, SecretNames.RconPassword,
            GameServerViewModel.RconPassword);
        await _repositoryApiClient.GameServersSecrets.UpdateGameServerSecret(accessToken, id, SecretNames.FtpUsername,
            GameServerViewModel.FtpUsername);
        await _repositoryApiClient.GameServersSecrets.UpdateGameServerSecret(accessToken, id, SecretNames.FtpPassword,
            GameServerViewModel.FtpPassword);

        return RedirectToPage("Index");
    }
}