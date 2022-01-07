using Microsoft.Azure.WebJobs;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using XtremeIdiots.Portal.DataLib;

namespace XtremeIdiots.Portal.RepositoryFunc
{
    public class DataMaintenance
    {
        public PortalDbContext Context { get; }

        public DataMaintenance(PortalDbContext context)
        {
            Context = context ?? throw new ArgumentNullException(nameof(context));
        }

        [FunctionName("DataMaintenance")]
        public void RunDataMaintenance([TimerTrigger("0 0 * * * *")] TimerInfo myTimer, ILogger log)
        {
            log.LogInformation($"Performing Data Maintenance");

            Context.Database.ExecuteSqlRawAsync($"DELETE FROM [dbo].[ChatMessages] WHERE [Timestamp] < CAST('{DateTime.UtcNow.AddMonths(-6):yyyy-MM-dd} 12:00:00' AS date)");
        }
    }
}
