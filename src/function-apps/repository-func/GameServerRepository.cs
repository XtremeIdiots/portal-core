using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using XtremeIdiots.Portal.DataLib;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace XtremeIdiots.Portal.RepositoryFunc
{
    public class GameServerRepository
    {
        public PortalDbContext Context { get; }

        public GameServerRepository(PortalDbContext context)
        {
            Context = context ?? throw new ArgumentNullException(nameof(context));
        }

        [FunctionName("GetGameServer")]
        public async Task<IActionResult> GetGameServer([HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req)
        {
            string id = req.Query["id"];

            if (string.IsNullOrWhiteSpace(id))
                return new BadRequestResult();

            var gameServer = await Context.GameServers.SingleOrDefaultAsync(gs => gs.Id == id);

            if (gameServer == null)
                return new NotFoundResult();

            return new OkObjectResult(gameServer);
        }

        [FunctionName("CreateGameServer")]
        public async Task<IActionResult> CreateGameServer([HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req)
        {
            var requestBody = await new StreamReader(req.Body).ReadToEndAsync();

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

            await Context.GameServers.AddAsync(gameServer);
            await Context.SaveChangesAsync();

            return new OkObjectResult(gameServer);
        }

        [FunctionName("UpdateGameServer")]
        public async Task<IActionResult> UpdateGameServer([HttpTrigger(AuthorizationLevel.Function, "patch", Route = null)] HttpRequest req)
        {
            var requestBody = await new StreamReader(req.Body).ReadToEndAsync();

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
