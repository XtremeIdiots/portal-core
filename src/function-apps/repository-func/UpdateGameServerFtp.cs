using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace XtremeIdiots.Portal.RepositoryFunc
{
    public class UpdateGameServerFtp
    {
        public UpdateGameServerFtp()
        {
            
        }

        [FunctionName("UpdateGameServerFtp")]
        public void Run([TimerTrigger("0 0 * * * *")]TimerInfo myTimer, ILogger log)
        {
            log.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");
        }
    }
}
