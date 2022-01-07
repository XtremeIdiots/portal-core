﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Net;
using XtremeIdiots.Portal.DataLib;

namespace repository_webapi.Controllers
{
    [ApiController]
    [Authorize(Roles = "ServiceAccount")]
    [Route("api/GameServer")]
    public class GameServerController : ControllerBase
    {
        public PortalDbContext Context { get; }

        public GameServerController(PortalDbContext context)
        {
            Context = context ?? throw new ArgumentNullException(nameof(context));
        }

        [HttpGet(Name = "GameServer")]
        public async Task<IActionResult> GetGameServer(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return new BadRequestResult();

            var gameServer = await Context.GameServers.SingleOrDefaultAsync(gs => gs.Id == id);

            if (gameServer == null)
                return new NotFoundResult();

            return new OkObjectResult(gameServer);
        }

        [HttpPost(Name = "GameServer")]
        public async Task<IActionResult> CreateGameServer()
        {
            var requestBody = await new StreamReader(Request.Body).ReadToEndAsync();

            GameServer gameServer;
            try
            {
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
                gameServer = JsonConvert.DeserializeObject<GameServer>(requestBody);
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex);
            }

            if (gameServer == null)
                return new BadRequestResult();

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

        [HttpPatch(Name = "GameServer")]
        public async Task<IActionResult> UpdateGameServer()
        {
            var requestBody = await new StreamReader(Request.Body).ReadToEndAsync();

            GameServer gameServer;
            try
            {
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
                gameServer = JsonConvert.DeserializeObject<GameServer>(requestBody);
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex);
            }

            if (gameServer == null)
                return new BadRequestResult();

            var gameServerToUpdate = await Context.GameServers.SingleOrDefaultAsync(gs => gs.Id == gameServer.Id);

            if (gameServerToUpdate == null)
                return new NotFoundResult();

            if (!string.IsNullOrWhiteSpace(gameServer.Title))
                gameServerToUpdate.Title = gameServer.Title.Trim();

#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
            if (IPAddress.TryParse(gameServerToUpdate.IpAddress, out IPAddress ip))
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
                gameServerToUpdate.IpAddress = ip.ToString();

            if (gameServerToUpdate.QueryPort != 0)
                gameServerToUpdate.QueryPort = gameServer.QueryPort;

            await Context.SaveChangesAsync();

            return new OkObjectResult(gameServerToUpdate);
        }
    }
}
