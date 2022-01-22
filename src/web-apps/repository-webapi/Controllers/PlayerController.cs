using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using XtremeIdiots.Portal.DataLib;

namespace XtremeIdiots.Portal.RepositoryWebApi.Controllers;

[ApiController]
[Authorize(Roles = "ServiceAccount,MgmtWebAdminUser")]
[Route("api/Player")]
public class PlayerController : ControllerBase
{
    public PlayerController(ILogger<PlayerController> log, PortalDbContext context)
    {
        Log = log ?? throw new ArgumentNullException(nameof(log));
        Context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public PortalDbContext Context { get; }
    public ILogger<PlayerController> Log { get; }

    [HttpGet(Name = "Player")]
    public async Task<IActionResult> GetPlayer()
    {
        string id = Request.Query["id"];
        string gameType = Request.Query["gameType"];
        string guid = Request.Query["guid"];

        if (!string.IsNullOrWhiteSpace(id))
        {
            if (!Guid.TryParse(id, out var idAsGuid)) return new BadRequestResult();

            var player = await Context.Players.SingleOrDefaultAsync(p => p.Id == idAsGuid);

            if (player == null) return new NotFoundResult();

            return new OkObjectResult(player);
        }
        else
        {
            if (string.IsNullOrWhiteSpace(gameType) || string.IsNullOrWhiteSpace(guid)) return new BadRequestResult();

            guid = guid.ToLower();

            var player = await Context.Players.SingleOrDefaultAsync(p => p.GameType == gameType && p.Guid == guid);

            if (player == null) return new NotFoundResult();

            return new OkObjectResult(player);
        }
    }

    [HttpPost(Name = "Player")]
    public async Task<IActionResult> CreatePlayer()
    {
        var requestBody = await new StreamReader(Request.Body).ReadToEndAsync();

        Player player;
        try
        {
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
            player = JsonConvert.DeserializeObject<Player>(requestBody);
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
        }
        catch (Exception ex)
        {
            return new BadRequestObjectResult(ex);
        }

        if (player == null) return new BadRequestResult();

        var existingPlayer =
            await Context.Players.SingleOrDefaultAsync(p => p.GameType == player.GameType && p.Guid == player.Guid);
        if (existingPlayer != null) return new ConflictObjectResult(existingPlayer);

        player.Username = player.Username.Trim();
        player.Guid = player.Guid.ToLower().Trim();

        player.FirstSeen = DateTime.UtcNow;
        player.LastSeen = DateTime.UtcNow;

        await Context.Players.AddAsync(player);
        await Context.SaveChangesAsync();

        return new OkObjectResult(player);
    }

    [HttpPatch(Name = "Player")]
    public async Task<IActionResult> UpdatePlayer()
    {
        var requestBody = await new StreamReader(Request.Body).ReadToEndAsync();

        Player player;
        try
        {
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
            player = JsonConvert.DeserializeObject<Player>(requestBody);
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
        }
        catch (Exception ex)
        {
            return new BadRequestObjectResult(ex);
        }

        if (player == null) return new BadRequestResult();

        var playerToUpdate = await Context.Players.SingleOrDefaultAsync(p => p.Id == player.Id);

        if (playerToUpdate == null) return new NotFoundResult();

        if (!string.IsNullOrWhiteSpace(player.Username)) playerToUpdate.Username = player.Username.Trim();

#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
        if (IPAddress.TryParse(playerToUpdate.IpAddress, out var ip))
        {
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
            playerToUpdate.IpAddress = ip.ToString();
        }

        playerToUpdate.LastSeen = DateTime.UtcNow;

        await Context.SaveChangesAsync();

        return new OkObjectResult(playerToUpdate);
    }
}