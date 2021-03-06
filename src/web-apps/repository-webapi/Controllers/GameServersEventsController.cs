using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using XtremeIdiots.Portal.DataLib;

namespace XtremeIdiots.Portal.RepositoryWebApi.Controllers;

[ApiController]
[Authorize(Roles = "ServiceAccount,MgmtWebAdminUser")]
public class GameServersEventsController : ControllerBase
{
    public GameServersEventsController(PortalDbContext context)
    {
        Context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public PortalDbContext Context { get; }

    [HttpPost]
    [Route("api/game-servers/{serverId}/event")]
    public async Task<IActionResult> CreateGameServerEvent(string serverId)
    {
        var requestBody = await new StreamReader(Request.Body).ReadToEndAsync();

        GameServerEvent gameServerEvent;
        try
        {
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
            gameServerEvent = JsonConvert.DeserializeObject<GameServerEvent>(requestBody);
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
        }
        catch (Exception ex)
        {
            return new BadRequestObjectResult(ex);
        }

        if (gameServerEvent == null) return new BadRequestResult();
        if (gameServerEvent.GameServerId != serverId) return new BadRequestResult();

        await Context.GameServerEvents.AddAsync(gameServerEvent);
        await Context.SaveChangesAsync();

        return new OkObjectResult(gameServerEvent);
    }
}