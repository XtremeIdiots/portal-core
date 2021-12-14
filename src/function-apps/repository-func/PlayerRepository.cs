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
using XtremeIdiots.Portal.DataLib.Models;
using System;

namespace XtremeIdiots.Portal.RepositoryFunc
{
    public class PlayerRepository
    {
        public PortalDbContext Context { get; }

        public PlayerRepository(PortalDbContext context)
        {
            Context = context ?? throw new System.ArgumentNullException(nameof(context));
        }

        [FunctionName("GetPlayer")]
        public async Task<IActionResult> GetPlayer([HttpTrigger(AuthorizationLevel.Function, "get", Route = null)] HttpRequest req)
        {
            var id = req.Query["id"];

            if (string.IsNullOrWhiteSpace(id))
                return new BadRequestResult();

            var player = await Context.Players.SingleOrDefaultAsync(p => p.Id == id);

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

            await Context.SaveChangesAsync();

            return new OkObjectResult(playerToUpdate);
        }
    }
}
