using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using XtremeIdiots.Portal.CommonLib.Models;

namespace XtremeIdiots.Portal.FunctionApp
{
    public static class GameEventsIngest
    {
        [FunctionName("OnSay")]
        [return: ServiceBus("chat_message_queue", Connection = "service-bus-connection-string")]
        public static string OnSay([HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] string input, ILogger log)
        {
            OnSay onSayEvent;
            try
            {
                onSayEvent = JsonConvert.DeserializeObject<OnSay>(input);
            }
            catch (Exception ex)
            {
                log.LogError($"OnSay Raw Input: '{input}'");
                log.LogError(ex, "OnSay was not in expected format");
                throw;
            }

            return JsonConvert.SerializeObject(onSayEvent);
        }

        [FunctionName("ProcessOnSay")]
        public static void ProcessOnSay(
        [ServiceBusTrigger("chat_message_queue", Connection = "service-bus-connection-string")] string myQueueItem, ILogger log)
        {
            OnSay onSayEvent;
            try
            {
                onSayEvent = JsonConvert.DeserializeObject<OnSay>(myQueueItem);
            }
            catch (Exception ex)
            {
                log.LogError(ex, "OnSay was not in expected format");
                throw;
            }

            log.LogInformation($"ProcessOnSay :: Username: '{onSayEvent.Username}', Guid: '{onSayEvent.Guid}', Message: '{onSayEvent.Message}'");
        }

        [FunctionName("OnMapChange")]
        [return: ServiceBus("map_change_queue", Connection = "service-bus-connection-string")]
        public static string OnMapChange([HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] string input, ILogger log)
        {
            OnMapChange onMapChangeEvent;
            try
            {
                onMapChangeEvent = JsonConvert.DeserializeObject<OnMapChange>(input);
            }
            catch (Exception ex)
            {
                log.LogError($"OnMapChange Raw Input: '{input}'");
                log.LogError(ex, "OnMapChange was not in expected format");
                throw;
            }

            return JsonConvert.SerializeObject(onMapChangeEvent);
        }

        [FunctionName("ProcessOnMapChange")]
        public static void ProcessOnMapChange(
        [ServiceBusTrigger("map_change_queue", Connection = "service-bus-connection-string")] string myQueueItem, ILogger log)
        {
            OnMapChange onMapChangeEvent;
            try
            {
                onMapChangeEvent = JsonConvert.DeserializeObject<OnMapChange>(myQueueItem);
            }
            catch (Exception ex)
            {
                log.LogError(ex, "OnSay was not in expected format");
                throw;
            }

            log.LogInformation($"ProcessOnMapChange :: GameName: '{onMapChangeEvent.GameName}', GameType: '{onMapChangeEvent.GameType}', MapName: '{onMapChangeEvent.MapName}'");
        }
    }
}
