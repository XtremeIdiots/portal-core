using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using XtremeIdiots.Portal.DataLib;

namespace XtremeIdiots.Portal.RepositoryWebApi.Controllers;

[ApiController]
[Authorize(Roles = "ServiceAccount")]
public class DataMaintenanceController : ControllerBase
{
    public DataMaintenanceController(PortalDbContext context)
    {
        Context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public PortalDbContext Context { get; }

    [HttpDelete]
    [Route("api/DataMaintenance/PruneChatMessages")]
    public async Task<IActionResult> PruneChatMessages()
    {
        await Context.Database.ExecuteSqlRawAsync(
            $"DELETE FROM [dbo].[ChatMessages] WHERE [Timestamp] < CAST('{DateTime.UtcNow.AddMonths(-6):yyyy-MM-dd} 12:00:00' AS date)");

        return new OkResult();
    }
}