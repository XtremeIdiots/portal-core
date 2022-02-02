using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using XtremeIdiots.Portal.DataLib;

namespace XtremeIdiots.Portal.RepositoryWebApi.Controllers;

[ApiController]
[Authorize(Roles = "ServiceAccount,MgmtWebAdminUser")]
public class GameServerSecretsController : ControllerBase
{
    private readonly IConfiguration _configuration;

    public GameServerSecretsController(PortalDbContext context, IConfiguration configuration)
    {
        _configuration = configuration;
        Context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public PortalDbContext Context { get; }

    [HttpGet]
    [Route("api/GameServer/{id}/secrets/{secret}")]
    public async Task<IActionResult> GetGameServerSecret(string id, string secret)
    {
        if (string.IsNullOrWhiteSpace(id)) return new BadRequestResult();

        var gameServer = await Context.GameServers.SingleOrDefaultAsync(gs => gs.Id == id);

        if (gameServer == null) return new BadRequestResult();

        var client = new SecretClient(new Uri(_configuration["gameservers-keyvault-endpoint"]), new DefaultAzureCredential());
        var secretValue = await client.GetSecretAsync(secret);

        if (secretValue == null) return new NotFoundResult();

        return new OkObjectResult(secretValue);
    }

    [HttpPost]
    [Route("api/GameServer/{id}/secrets/{secret}")]
    public async Task<IActionResult> CreateGameServerSecret(string id, string secret)
    {
        if (string.IsNullOrWhiteSpace(id)) return new BadRequestResult();

        var secretValue = await new StreamReader(Request.Body).ReadToEndAsync();

        if (string.IsNullOrWhiteSpace(secretValue)) return new BadRequestResult();

        var gameServer = await Context.GameServers.SingleOrDefaultAsync(gs => gs.Id == id);

        if (gameServer == null) return new BadRequestResult();

        var client = new SecretClient(new Uri(_configuration["gameservers-keyvault-endpoint"]), new DefaultAzureCredential());

        var response = await client.SetSecretAsync(secret, secretValue);

        return new OkObjectResult(response);
    }

    [HttpPatch]
    [Route("api/GameServer/{id}/secrets/{secret}")]
    public async Task<IActionResult> UpdateGameServerSecret(string id, string secret)
    {
        if (string.IsNullOrWhiteSpace(id)) return new BadRequestResult();

        var newSecretValue = await new StreamReader(Request.Body).ReadToEndAsync();

        if (string.IsNullOrWhiteSpace(newSecretValue)) return new BadRequestResult();

        var gameServer = await Context.GameServers.SingleOrDefaultAsync(gs => gs.Id == id);

        if (gameServer == null) return new BadRequestResult();

        var client = new SecretClient(new Uri(_configuration["gameservers-keyvault-endpoint"]), new DefaultAzureCredential());
        var secretValue = await client.GetSecretAsync(secret);

        if (secretValue != null) return new BadRequestResult();

        var response = await client.SetSecretAsync(secret, newSecretValue);

        return new OkObjectResult(response);
    }
}