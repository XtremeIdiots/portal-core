using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using XtremeIdiots.Portal.DataLib;

namespace XtremeIdiots.Portal.RepositoryWebApi.Controllers
{
    [ApiController]
    [Authorize(Roles = "ServiceAccount,MgmtWebAdminUser")]
    public class GameServersController : Controller
    {
        public GameServersController(PortalDbContext context)
        {
            Context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public PortalDbContext Context { get; }

        [HttpGet]
        [Route("api/game-servers")]
        public async Task<IActionResult> GetGameServer()
        {
            var gameServers = await Context.GameServers.ToListAsync();

            return new OkObjectResult(gameServers);
        }
    }
}
