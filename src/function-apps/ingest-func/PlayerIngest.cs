using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Net;
using XtremeIdiots.Portal.CommonLib.Models;
using XtremeIdiots.Portal.DataLib;

namespace XtremeIdiots.Portal.FunctionApp
{
    public class PlayerIngest
    {
        [FunctionName("OnPlayerConnected")]
        [return: ServiceBus("player_connected_queue", Connection = "service-bus-connection-string")]
        public string OnPlayerConnected([HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] string input, ILogger log)
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
        public void ProcessOnPlayerConnected(
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

            var baseUrl = Environment.GetEnvironmentVariable("apim-base-url");
            var subscriptionKey = Environment.GetEnvironmentVariable("apim-subscription-key");

            var getPlayerRequest = new RestRequest("player-repository/GetPlayerByGame", Method.GET);
            getPlayerRequest.AddHeader("Ocp-Apim-Subscription-Key", subscriptionKey);
            getPlayerRequest.AddParameter(new Parameter("gameType", playerConnectedEvent.GameType, ParameterType.QueryString));
            getPlayerRequest.AddParameter(new Parameter("guid", playerConnectedEvent.Guid, ParameterType.QueryString));

            var client = new RestClient(baseUrl);
            var response = client.Execute(getPlayerRequest);

            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                var player = new Player()
                {
                    GameType = playerConnectedEvent.GameType,
                    Guid = playerConnectedEvent.Guid,
                    Username = playerConnectedEvent.Username
                };

                var createPlayerRequest = new RestRequest("player-repository/CreatePlayer", Method.POST);
                createPlayerRequest.AddHeader("Ocp-Apim-Subscription-Key", subscriptionKey);
                createPlayerRequest.AddJsonBody(player);

                client.Execute(createPlayerRequest);
            } 
            else if (response.IsSuccessful)
            {

            }

            // Get Player

            // If player doesn't exist create player

            // If event not stale and info to update then update player
        }
    }
}
