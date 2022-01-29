using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Identity.Web;
using Newtonsoft.Json;
using RestSharp;
using XtremeIdiots.Portal.DataLib;
using XtremeIdiots.Portal.MgmtWeb.ViewModels;

namespace XtremeIdiots.Portal.MgmtWeb.Pages.GameServers;

[Authorize(Roles = "ApplicationUser")]
[AuthorizeForScopes(ScopeKeySection = "web-api-repository-scope")]
public class EditModel : PageModel
{
    private readonly IConfiguration _configuration;
    private readonly ITokenAcquisition _tokenAcquisition;

    public EditModel(IConfiguration configuration, ITokenAcquisition tokenAcquisition)
    {
        _configuration = configuration;
        _tokenAcquisition = tokenAcquisition;
    }

    [BindProperty] public GameServerViewModel GameServerViewModel { get; set; }

    public async Task<IActionResult> OnGet(string? id)
    {
        if (string.IsNullOrWhiteSpace(id)) return NotFound();

        var gameServer = await GetGameServer(id);

        if (gameServer == null)
            return NotFound();

        GameServerViewModel = new GameServerViewModel
        {
            Id = gameServer.Id,
            Title = gameServer.Title,
            GameType = gameServer.GameType,
            IpAddress = gameServer.IpAddress,
            QueryPort = gameServer.QueryPort
        };

        return Page();
    }

    public async Task<IActionResult> OnPostAsync(string id)
    {
        if (!ModelState.IsValid) return Page();

        var gameServer = await GetGameServer(id);

        if (gameServer == null)
            return NotFound();

        gameServer.Title = GameServerViewModel.Title;
        gameServer.GameType = GameServerViewModel.GameType;
        gameServer.IpAddress = GameServerViewModel.IpAddress;
        gameServer.QueryPort = GameServerViewModel.QueryPort;

        await UpdateGameServer(gameServer);

        return RedirectToPage("Index");
    }

    private async Task<GameServer?> GetGameServer(string id)
    {
        var client = new RestClient(_configuration["apim-base-url"]);
        var request = new RestRequest("repository/GameServer");

        string[] scopes = {_configuration["web-api-repository-scope"]};
        var accessToken = await _tokenAcquisition.GetAccessTokenForUserAsync(scopes);

        request.AddHeader("Ocp-Apim-Subscription-Key", _configuration["apim-subscription-key"]);
        request.AddHeader("Authorization", $"Bearer {accessToken}");
        request.AddParameter(new QueryParameter("id", id));

        var response = await client.ExecuteAsync(request);

        if (response.IsSuccessful)
            return JsonConvert.DeserializeObject<GameServer>(response.Content);

        if (response.StatusCode == HttpStatusCode.NotFound)
            return null;

        throw new Exception("Failed to execute 'repository/GameServer'");
    }

    private async Task UpdateGameServer(GameServer gameServer)
    {
        var client = new RestClient(_configuration["apim-base-url"]);
        var request = new RestRequest("repository/GameServer", Method.Patch);

        string[] scopes = {_configuration["web-api-repository-scope"]};
        var accessToken = await _tokenAcquisition.GetAccessTokenForUserAsync(scopes);

        request.AddHeader("Ocp-Apim-Subscription-Key", _configuration["apim-subscription-key"]);
        request.AddHeader("Authorization", $"Bearer {accessToken}");
        request.AddJsonBody(gameServer);

        await client.ExecuteAsync(request);
    }
}