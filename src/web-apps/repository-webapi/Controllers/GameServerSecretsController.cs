﻿using Azure;
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

        var client = new SecretClient(new Uri(_configuration["gameservers-keyvault-endpoint"]),
            new DefaultAzureCredential());

        try
        {
            var keyVaultResponse = await client.GetSecretAsync($"{id}-{secret}");
            return new OkObjectResult(keyVaultResponse.Value);
        }
        catch (RequestFailedException ex)
        {
            if (ex.Status == 404)
                return new NotFoundResult();

            throw;
        }
    }

    [HttpPost]
    [Route("api/GameServer/{id}/secrets/{secret}")]
    public async Task<IActionResult> SetGameServerSecret(string id, string secret)
    {
        if (string.IsNullOrWhiteSpace(id)) return new BadRequestResult();

        var gameServer = await Context.GameServers.SingleOrDefaultAsync(gs => gs.Id == id);
        if (gameServer == null) return new BadRequestResult();

        var client = new SecretClient(new Uri(_configuration["gameservers-keyvault-endpoint"]),
            new DefaultAzureCredential());

        var rawSecretValue = await new StreamReader(Request.Body).ReadToEndAsync();

        try
        {
            var keyVaultResponse = await client.GetSecretAsync($"{id}-{secret}");

            if (keyVaultResponse.Value.Value != rawSecretValue)
                keyVaultResponse = await client.SetSecretAsync($"{id}-{secret}", rawSecretValue);

            return new OkObjectResult(keyVaultResponse.Value);
        }
        catch (RequestFailedException ex)
        {
            if (ex.Status != 404)
                throw;
        }

        var newSecretKeyVaultResponse = await client.SetSecretAsync($"{id}-{secret}", rawSecretValue);
        return new OkObjectResult(newSecretKeyVaultResponse.Value);
    }
}