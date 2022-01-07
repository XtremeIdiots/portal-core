using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Net;
using System.Threading.Tasks;
using XtremeIdiots.Portal.CommonLib.Models;
using XtremeIdiots.Portal.DataLib;
using XtremeIdiots.Portal.IngestFunc.Providers;

namespace XtremeIdiots.Portal.FunctionApp
{
    public class PlayerEventsIngest
    {
        public PlayerEventsIngest(IRepositoryTokenProvider repositoryTokenProvider)
        {
            RepositoryTokenProvider = repositoryTokenProvider;
        }

        private IRepositoryTokenProvider RepositoryTokenProvider { get; }
        private string ApimBaseUrl => Environment.GetEnvironmentVariable("apim-base-url");
        private string ApimSubscriptionKey => Environment.GetEnvironmentVariable("apim-subscription-key");

        [FunctionName("ProcessOnPlayerConnected")]
        public async Task ProcessOnPlayerConnected([ServiceBusTrigger("player_connected_queue", Connection = "service-bus-connection-string")] string myQueueItem, ILogger log)
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

            var existingPlayer = await GetPlayer(playerConnectedEvent.GameType, playerConnectedEvent.Guid);

            if (existingPlayer == null)
            {
                var player = new Player()
                {
                    GameType = playerConnectedEvent.GameType,
                    Guid = playerConnectedEvent.Guid,
                    Username = playerConnectedEvent.Username,
                    IpAddress = playerConnectedEvent.IpAddress
                };

                await CreatePlayer(player);
            }
            else
            {
                if (playerConnectedEvent.EventGeneratedUtc > existingPlayer.LastSeen)
                {
                    existingPlayer.Username = playerConnectedEvent.Username;
                    existingPlayer.IpAddress = playerConnectedEvent.IpAddress;

                    await UpdatePlayer(existingPlayer);
                }
            }
        }

        [FunctionName("ProcessOnChatMessage")]
        public async Task ProcessOnChatMessage([ServiceBusTrigger("chat_message_queue", Connection = "service-bus-connection-string")] string myQueueItem, ILogger log)
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

            var player = await GetPlayer(onChatMessage.GameType, onChatMessage.Guid);

            if (player != null)
            {
                var chatMessage = new ChatMessage()
                {
                    GameServerId = onChatMessage.ServerId,
                    PlayerId = player.Id,
                    Username = onChatMessage.Username,
                    Message = onChatMessage.Message,
                    Type = onChatMessage.Type,
                    Timestamp = onChatMessage.EventGeneratedUtc
                };

                await CreateChatMessage(chatMessage);
            }
        }

        private async Task<Player> GetPlayer(string gameType, string guid)
        {
            var client = new RestClient(ApimBaseUrl);
            var request = new RestRequest("player-repository/GetPlayerByGame", Method.Get);
            var accessToken = await RepositoryTokenProvider.GetRepositoryAccessToken();

            request.AddHeader("Ocp-Apim-Subscription-Key", ApimSubscriptionKey);
            request.AddHeader("Authorization", $"Bearer {accessToken}");
            request.AddParameter(new QueryParameter("gameType", gameType));
            request.AddParameter(new QueryParameter("guid", guid));

            var response = await client.ExecuteAsync(request);

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

        private async Task CreatePlayer(Player player)
        {
            var client = new RestClient(ApimBaseUrl);
            var request = new RestRequest("player-repository/CreatePlayer", Method.Post);
            var accessToken = await RepositoryTokenProvider.GetRepositoryAccessToken();

            request.AddHeader("Ocp-Apim-Subscription-Key", ApimSubscriptionKey);
            request.AddHeader("Authorization", $"Bearer {accessToken}");
            request.AddJsonBody(player);

            await client.ExecuteAsync(request);
        }

        private async Task UpdatePlayer(Player player)
        {
            var client = new RestClient(ApimBaseUrl);
            var request = new RestRequest("player-repository/UpdatePlayer", Method.Patch);
            var accessToken = await RepositoryTokenProvider.GetRepositoryAccessToken();

            request.AddHeader("Ocp-Apim-Subscription-Key", ApimSubscriptionKey);
            request.AddHeader("Authorization", $"Bearer {accessToken}");
            request.AddJsonBody(player);

            await client.ExecuteAsync(request);
        }

        private async Task CreateChatMessage(ChatMessage chatMessage)
        {
            var client = new RestClient(ApimBaseUrl);
            var request = new RestRequest("chat-message-repository/CreateChatMessage", Method.Post);
            var accessToken = await RepositoryTokenProvider.GetRepositoryAccessToken();

            request.AddHeader("Ocp-Apim-Subscription-Key", ApimSubscriptionKey);
            request.AddHeader("Authorization", $"Bearer {accessToken}");
            request.AddJsonBody(chatMessage);

            await client.ExecuteAsync(request);
        }
    }
}
