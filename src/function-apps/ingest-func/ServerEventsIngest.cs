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
        private static string WebApiPortalApplicationAudience => Environment.GetEnvironmentVariable("webapi-portal-application-audience");

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

            var existingServer = await GetGameServer(log, onServerConnected.Id);

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
            var request = new RestRequest("game-server-repository/GameServer", Method.Get);
            var accessToken = await GetRepositoryAccessToken(log);

            request.AddHeader("Ocp-Apim-Subscription-Key", ApimSubscriptionKey);
            request.AddHeader("Authorization", $"Bearer {accessToken}");
            request.AddParameter(new QueryParameter("id", id));

            var response = await client.ExecuteAsync(request);

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
            var request = new RestRequest("game-server-repository/GameServer", Method.Post);
            var accessToken = await GetRepositoryAccessToken(log);

            request.AddHeader("Ocp-Apim-Subscription-Key", ApimSubscriptionKey);
            request.AddHeader("Authorization", $"Bearer {accessToken}");
            request.AddJsonBody(gameServer);

            await client.ExecuteAsync(request);
        }

        private static async Task<string> GetRepositoryAccessToken(ILogger log)
        {
            var tokenCredential = new ManagedIdentityCredential();

            AccessToken accessToken;
            try
            {
                accessToken = await tokenCredential.GetTokenAsync(
                    new TokenRequestContext(scopes: new string[] { $"{WebApiPortalApplicationAudience}/.default" }) { }
                );

                log.LogInformation($"AccessToken: {accessToken.Token}");
            }
            catch (Exception ex)
            {
                log.LogError(ex, "Failed to get managed identity token");
                throw;
            }

            return accessToken.Token;
        }
    }
}
