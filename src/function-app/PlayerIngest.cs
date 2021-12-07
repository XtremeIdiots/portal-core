using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using XtremeIdiots.Portal.CommonLib.Models;

namespace XtremeIdiots.Portal.FunctionApp
{
    public static class PlayerIngest
    {
        [FunctionName("OnPlayerConnected")]
        [return: ServiceBus("player_connected_queue", Connection = "service-bus-connection-string")]
        public static string OnPlayerConnected([HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] string input, ILogger log)
        {
            OnPlayerConnected playerConnectedEvent;
            try
            {
                playerConnectedEvent = JsonConvert.DeserializeObject<OnPlayerConnected>(input);
            }
            catch (Exception ex)
            {
                log.LogError($"OnPlayerConnected Raw Input: '{input}'");
                log.LogError(ex, "OnPlayerConnected was not in expected format");
                throw;
            }

            return JsonConvert.SerializeObject(playerConnectedEvent);
        }

        [FunctionName("ProcessOnPlayerConnected")]
        public static void ProcessOnPlayerConnected(
        [ServiceBusTrigger("player_connected_queue", Connection = "service-bus-connection-string")] string myQueueItem, ILogger log)
        {
            OnPlayerConnected playerConnectedEvent;
            try
            {
                playerConnectedEvent = JsonConvert.DeserializeObject<OnPlayerConnected>(myQueueItem);
            }
            catch (Exception ex)
            {
                log.LogError(ex, "OnPlayerConnected was not in expected format");
                throw;
            }

            log.LogInformation($"ProcessOnPlayerConnected :: Username: '{playerConnectedEvent.Username}', Guid: '{playerConnectedEvent.Guid}'");
        }
    }
}
