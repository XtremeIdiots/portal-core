using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using XtremeIdiots.Portal.CommonLib.Events;
using XtremeIdiots.Portal.DataLib;
using RestSharp;
using System.Net;
using XtremeIdiots.Portal.CommonLib.Models;
using Azure.Identity;
using System.Threading.Tasks;
using Azure.Core;

namespace XtremeIdiots.Portal.IngestFunc
{
    public class ServerEventsIngest
    {
        private static string ApimBaseUrl => Environment.GetEnvironmentVariable("apim-base-url");
        private static string ApimSubscriptionKey => Environment.GetEnvironmentVariable("apim-subscription-key");
        private static string WebApiPortalRepositoryApplicationName => Environment.GetEnvironmentVariable("webapi-portal-repository-name");

        [FunctionName("ProcessOnServerConnected")]
        public async Task ProcessOnServerConnected([ServiceBusTrigger("server_connected_queue", Connection = "service-bus-connection-string")] string myQueueItem, ILogger log)
        {
            OnServerConnected onServerConnected;
            try
            {
                onServerConnected = JsonConvert.DeserializeObject<OnServerConnected>(myQueueItem);
            }
            catch (Exception ex)
            {
                log.LogError(ex, "OnServerConnected was not in expected format");
                throw;
            }

            log.LogInformation($"OnServerConnected :: Id: '{onServerConnected.Id}', GameType: '{onServerConnected.GameType}'");

            var existingServer = GetGameServer(log, onServerConnected.Id);

            if (existingServer == null)
            {
                var gameServer = new GameServer()
                {
                    Id = onServerConnected.Id,
                    GameType = onServerConnected.GameType
                };

                await CreateGameServer(log, gameServer);
            }
        }

        [FunctionName("ProcessOnMapChange")]
        public static void ProcessOnMapChange([ServiceBusTrigger("map_change_queue", Connection = "service-bus-connection-string")] string myQueueItem, ILogger log)
        {
            OnMapChange onMapChange;
            try
            {
                onMapChange = JsonConvert.DeserializeObject<OnMapChange>(myQueueItem);
            }
            catch (Exception ex)
            {
                log.LogError(ex, "OnMapChange was not in expected format");
                throw;
            }

            log.LogInformation($"ProcessOnMapChange :: GameName: '{onMapChange.GameName}', GameType: '{onMapChange.GameType}', MapName: '{onMapChange.MapName}'");
        }

        private async static Task<GameServer> GetGameServer(ILogger log, string id)
        {
            var client = new RestClient(ApimBaseUrl);
            var request = new RestRequest("game-server-repository/GameServer", Method.GET);
            var accessToken = await GetRepositoryAccessToken(log);

            request.AddHeader("Ocp-Apim-Subscription-Key", ApimSubscriptionKey);
            request.AddHeader("Authorization", $"Bearer {accessToken}");
            request.AddParameter(new Parameter("id", id, ParameterType.QueryString));

            var response = client.Execute(request);

            if (response.IsSuccessful)
            {
                return JsonConvert.DeserializeObject<GameServer>(response.Content);
            }
            else if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return null;
            }
            else
            {
                throw new Exception("Failed to execute 'game-server-repository/GameServer'");
            }
        }

        private async static Task CreateGameServer(ILogger log, GameServer gameServer)
        {
            var client = new RestClient(ApimBaseUrl);
            var request = new RestRequest("game-server-repository/GameServer", Method.POST);
            var accessToken = await GetRepositoryAccessToken(log);

            request.AddHeader("Ocp-Apim-Subscription-Key", ApimSubscriptionKey);
            request.AddHeader("Authorization", $"Bearer {accessToken}");
            request.AddJsonBody(gameServer);

            client.Execute(request);
        }

        private static async Task<string> GetRepositoryAccessToken(ILogger log)
        {
            var tokenCredential = new ManagedIdentityCredential();

            AccessToken accessToken;
            try
            {
                accessToken = await tokenCredential.GetTokenAsync(
                    new TokenRequestContext() { }
                );

                log.LogInformation($"AccessToken: {accessToken}");
            }
            catch (Exception ex)
            {
                log.LogError(ex, "Failed to get managed identity token");
                throw;
            }

            return accessToken.ToString();
        }
    }
}
