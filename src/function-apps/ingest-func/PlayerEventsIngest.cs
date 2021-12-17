using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Net;
using XtremeIdiots.Portal.CommonLib.Models;
using XtremeIdiots.Portal.DataLib;

namespace XtremeIdiots.Portal.FunctionApp
{
    public class PlayerEventsIngest
    {
        private static string ApimBaseUrl => Environment.GetEnvironmentVariable("apim-base-url");
        private static string ApimSubscriptionKey => Environment.GetEnvironmentVariable("apim-subscription-key");

        [FunctionName("ProcessOnPlayerConnected")]
        public void ProcessOnPlayerConnected([ServiceBusTrigger("player_connected_queue", Connection = "service-bus-connection-string")] string myQueueItem, ILogger log)
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

            var existingPlayer = GetPlayer(playerConnectedEvent.GameType, playerConnectedEvent.Guid);

            if (existingPlayer == null)
            {
                var player = new Player()
                {
                    GameType = playerConnectedEvent.GameType,
                    Guid = playerConnectedEvent.Guid,
                    Username = playerConnectedEvent.Username,
                    IpAddress = playerConnectedEvent.IpAddress
                };

                CreatePlayer(player);
            }
            else
            {
                if (playerConnectedEvent.EventGeneratedUtc > existingPlayer.LastSeen)
                {
                    existingPlayer.Username = playerConnectedEvent.Username;
                    existingPlayer.IpAddress = playerConnectedEvent.IpAddress;

                    UpdatePlayer(existingPlayer);
                }
            }
        }

        [FunctionName("ProcessOnChatMessage")]
        public static void ProcessOnChatMessage([ServiceBusTrigger("chat_message_queue", Connection = "service-bus-connection-string")] string myQueueItem, ILogger log)
        {
            OnChatMessage onChatMessage;
            try
            {
                onChatMessage = JsonConvert.DeserializeObject<OnChatMessage>(myQueueItem);
            }
            catch (Exception ex)
            {
                log.LogError(ex, "OnChatMessage was not in expected format");
                throw;
            }

            log.LogInformation($"ProcessOnChatMessage :: Username: '{onChatMessage.Username}', Guid: '{onChatMessage.Guid}', Message: '{onChatMessage.Message}'");
        }

        private static Player GetPlayer(string gameType, string guid)
        {
            var client = new RestClient(ApimBaseUrl);
            var request = new RestRequest("player-repository/GetPlayerByGame", Method.GET);
            request.AddHeader("Ocp-Apim-Subscription-Key", ApimSubscriptionKey);
            request.AddParameter(new Parameter("gameType", gameType, ParameterType.QueryString));
            request.AddParameter(new Parameter("guid", guid, ParameterType.QueryString));

            var response = client.Execute(request);

            if (response.IsSuccessful)
            {
                return JsonConvert.DeserializeObject<Player>(response.Content);
            }
            else if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return null;
            }
            else
            {
                throw new Exception("Failed to execute 'player-repository/GetPlayerByGame'");
            }
        }

        private static void CreatePlayer(Player player)
        {
            var client = new RestClient(ApimBaseUrl);
            var request = new RestRequest("player-repository/CreatePlayer", Method.POST);
            request.AddHeader("Ocp-Apim-Subscription-Key", ApimSubscriptionKey);
            request.AddJsonBody(player);

            client.Execute(request);
        }

        private static void UpdatePlayer(Player player)
        {
            var client = new RestClient(ApimBaseUrl);
            var request = new RestRequest("player-repository/UpdatePlayer", Method.PATCH);
            request.AddHeader("Ocp-Apim-Subscription-Key", ApimSubscriptionKey);
            request.AddJsonBody(player);

            client.Execute(request);
        }
    }
}
