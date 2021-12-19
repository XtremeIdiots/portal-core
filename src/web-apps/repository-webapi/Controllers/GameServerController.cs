using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Net;
using XtremeIdiots.Portal.DataLib;

namespace repository_webapi.Controllers
{
    [ApiController]
    [Route("api/game-server-repository")]
    public class GameServerController : ControllerBase
    {
        public PortalDbContext Context { get; }

        public GameServerController(PortalDbContext context)
        {
            Context = context ?? throw new ArgumentNullException(nameof(context));
        }

        [HttpGet(Name = "GetGameServer")]
        public async Task<IActionResult> GetGameServer()
        {
            string id = Request.Query["id"];

            if (string.IsNullOrWhiteSpace(id))
                return new BadRequestResult();

            var gameServer = await Context.GameServers.SingleOrDefaultAsync(gs => gs.Id == id);

            if (gameServer == null)
                return new NotFoundResult();

            return new OkObjectResult(gameServer);
        }

        [HttpPost(Name = "CreateGameServer")]
        public async Task<IActionResult> CreateGameServer()
        {
            var requestBody = await new StreamReader(Request.Body).ReadToEndAsync();

            GameServer gameServer;
            try
            {
                gameServer = JsonConvert.DeserializeObject<GameServer>(requestBody);
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex);
            }

            var existingGameServer = await Context.GameServers.SingleOrDefaultAsync(gs => gs.Id == gameServer.Id);
            if (existingGameServer != null)
                return new ConflictObjectResult(existingGameServer);

            if (string.IsNullOrWhiteSpace(gameServer.Title))
                gameServer.Title = "to-be-updated";

            if (string.IsNullOrWhiteSpace(gameServer.IpAddress))
                gameServer.IpAddress = "127.0.0.1";

            await Context.GameServers.AddAsync(gameServer);
            await Context.SaveChangesAsync();

            return new OkObjectResult(gameServer);
        }

        [HttpPatch(Name = "UpdateGameServer")]
        public async Task<IActionResult> UpdateGameServer()
        {
            var requestBody = await new StreamReader(Request.Body).ReadToEndAsync();

            GameServer gameServer;
            try
            {
                gameServer = JsonConvert.DeserializeObject<GameServer>(requestBody);
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex);
            }

            var gameServerToUpdate = await Context.GameServers.SingleOrDefaultAsync(gs => gs.Id == gameServer.Id);

            if (gameServerToUpdate == null)
                return new NotFoundResult();

            if (!string.IsNullOrWhiteSpace(gameServer.Title))
                gameServerToUpdate.Title = gameServer.Title.Trim();

            if (IPAddress.TryParse(gameServerToUpdate.IpAddress, out IPAddress ip))
                gameServerToUpdate.IpAddress = ip.ToString();

            if (gameServerToUpdate.QueryPort != 0)
                gameServerToUpdate.QueryPort = gameServer.QueryPort;

            await Context.SaveChangesAsync();

            return new OkObjectResult(gameServerToUpdate);
        }
    }
}
