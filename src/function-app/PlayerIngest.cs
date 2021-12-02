using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace XtremeIdiots.PlayerIngest
{
    public static class PlayerIngest
    {
        [FunctionName("OnPlayerConnected")]
        public static async Task<IActionResult> OnPlayerConnected(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("PlayerIngest - OnPlayerConnected Executed");

            return new OkResult();
        }
    }
}
