using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using XtremeIdiots.Portal.CommonLib.Events;
using XtremeIdiots.Portal.DataLib;
using RestSharp;
using System.Net;

namespace XtremeIdiots.Portal.IngestFunc
{
    public class ServerRegisterIngest
    {
        private static string ApimBaseUrl => Environment.GetEnvironmentVariable("apim-base-url");
        private static string ApimSubscriptionKey => Environment.GetEnvironmentVariable("apim-subscription-key");

        [FunctionName("OnRegisterServer")]
        [return: ServiceBus("server_register_queue", Connection = "service-bus-connection-string")]
        public string OnRegisterServer([HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] string input, ILogger log)
        {
            OnRegisterServer serverRegisterEvent;
            try
            {
                serverRegisterEvent = JsonConvert.DeserializeObject<OnRegisterServer>(input);
            }
            catch (Exception ex)
            {
                log.LogError($"OnRegisterServer Raw Input: '{input}'");
                log.LogError(ex, "OnRegisterServer was not in expected format");
                throw;
            }

            return JsonConvert.SerializeObject(serverRegisterEvent);
        }

        [FunctionName("ProcessOnRegisterServer")]
        public void ProcessOnRegisterServer(
        [ServiceBusTrigger("server_register_queue", Connection = "service-bus-connection-string")] string myQueueItem, ILogger log)
        {
            OnRegisterServer registerServerEvent;
            try
            {
                registerServerEvent = JsonConvert.DeserializeObject<OnRegisterServer>(myQueueItem);
            }
            catch (Exception ex)
            {
                log.LogError(ex, "OnRegisterServer was not in expected format");
                throw;
            }

            log.LogInformation($"ProcessOnRegisterServer :: Id: '{registerServerEvent.Id}', GameType: '{registerServerEvent.GameType}'");

            var existingServer = GetGameServer(registerServerEvent.Id);

            if (existingServer == null)
            {
                var gameServer = new GameServer()
                {
                    Id = registerServerEvent.Id,
                    GameType = registerServerEvent.GameType
                };

                CreateGameServer(gameServer);
            }
        }

        private static GameServer GetGameServer(string id)
        {
            var client = new RestClient(ApimBaseUrl);
            var request = new RestRequest("game-server-repository/GetGameServer", Method.GET);
            request.AddHeader("Ocp-Apim-Subscription-Key", ApimSubscriptionKey);
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
                throw new Exception("Failed to execute 'game-server-repository/GetGameServer'");
            }
        }

        private static void CreateGameServer(GameServer gameServer)
        {
            var client = new RestClient(ApimBaseUrl);
            var request = new RestRequest("game-server-repository/CreateGameServer", Method.POST);
            request.AddHeader("Ocp-Apim-Subscription-Key", ApimSubscriptionKey);
            request.AddJsonBody(gameServer);

            client.Execute(request);
        }
    }
}
