using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using XtremeIdiots.Portal.DataLib;
using Microsoft.EntityFrameworkCore;
using System;

namespace XtremeIdiots.Portal.RepositoryFunc
{
    public class PlayerRepository
    {
        public PortalDbContext Context { get; }

        public PlayerRepository(PortalDbContext context)
        {
            Context = context ?? throw new ArgumentNullException(nameof(context));
        }

        [FunctionName("GetPlayer")]
        public async Task<IActionResult> GetPlayer([HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req)
        {
            string id = req.Query["id"];

            if (string.IsNullOrWhiteSpace(id))
                return new BadRequestResult();

            if (!Guid.TryParse(id, out Guid idAsGuid))
                return new BadRequestResult();

            var player = await Context.Players.SingleOrDefaultAsync(p => p.Id == idAsGuid);

            if (player == null)
                return new NotFoundResult();

            return new OkObjectResult(player);
        }

        [FunctionName("GetPlayerByGame")]
        public async Task<IActionResult> GetPlayerByGame([HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req)
        {
            string gameType = req.Query["gameType"];
            string guid = req.Query["guid"];

            if (string.IsNullOrWhiteSpace(gameType) || string.IsNullOrWhiteSpace(guid))
                return new BadRequestResult();

            var player = await Context.Players.SingleOrDefaultAsync(p => p.GameType == gameType && p.Guid == guid);

            if (player == null)
                return new NotFoundResult();

            return new OkObjectResult(player);
        }

        [FunctionName("CreatePlayer")]
        public async Task<IActionResult> CreatePlayer([HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req)
        {
            var requestBody = await new StreamReader(req.Body).ReadToEndAsync();

            Player player;
            try
            {
                player = JsonConvert.DeserializeObject<Player>(requestBody);
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex);
            }

            var existingPlayer = await Context.Players.SingleOrDefaultAsync(p => p.GameType ==  player.GameType && p.Guid == player.Guid);
            if (existingPlayer != null)
                return new ConflictObjectResult(existingPlayer);

            player.FirstSeen = DateTime.UtcNow;
            player.LastSeen = DateTime.UtcNow;

            await Context.Players.AddAsync(player);
            await Context.SaveChangesAsync();

            return new OkObjectResult(player);
        }

        [FunctionName("UpdatePlayer")]
        public async Task<IActionResult> UpdatePlayer([HttpTrigger(AuthorizationLevel.Function, "patch", Route = null)] HttpRequest req)
        {
            var requestBody = await new StreamReader(req.Body).ReadToEndAsync();

            Player player;
            try
            {
                player = JsonConvert.DeserializeObject<Player>(requestBody);
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex);
            }

            var playerToUpdate = await Context.Players.SingleOrDefaultAsync(p => p.Id == player.Id);

            if (playerToUpdate == null)
                return new NotFoundResult();

            playerToUpdate.Username = player.Username;
            playerToUpdate.FirstSeen = player.FirstSeen;
            playerToUpdate.LastSeen = player.LastSeen;
            playerToUpdate.IpAddress = player.IpAddress;
            playerToUpdate.LastSeen = DateTime.UtcNow;

            await Context.SaveChangesAsync();

            return new OkObjectResult(playerToUpdate);
        }
    }
}
