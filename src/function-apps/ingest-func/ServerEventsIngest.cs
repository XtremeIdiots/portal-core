using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RestSharp;
using XtremeIdiots.Portal.CommonLib.Events;
using XtremeIdiots.Portal.DataLib;
using XtremeIdiots.Portal.IngestFunc.Providers;

namespace XtremeIdiots.Portal.IngestFunc;

public class ServerEventsIngest
{
    public ServerEventsIngest(IRepositoryTokenProvider repositoryTokenProvider)
    {
        RepositoryTokenProvider = repositoryTokenProvider;
    }

    private IRepositoryTokenProvider RepositoryTokenProvider { get; }
    private string ApimBaseUrl => Environment.GetEnvironmentVariable("apim-base-url");
    private string ApimSubscriptionKey => Environment.GetEnvironmentVariable("apim-subscription-key");

    [FunctionName("ProcessOnServerConnected")]
    public async Task ProcessOnServerConnected(
        [ServiceBusTrigger("server_connected_queue", Connection = "service-bus-connection-string")] string myQueueItem,
        ILogger log)
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

        log.LogInformation(
            $"OnServerConnected :: Id: '{onServerConnected.Id}', GameType: '{onServerConnected.GameType}'");

        var existingServer = await GetGameServer(log, onServerConnected.Id);

        if (existingServer == null)
        {
            var gameServer = new GameServer
            {
                Id = onServerConnected.Id,
                GameType = onServerConnected.GameType
            };

            await CreateGameServer(log, gameServer);
        }
    }

    [FunctionName("ProcessOnMapChange")]
    public void ProcessOnMapChange(
        [ServiceBusTrigger("map_change_queue", Connection = "service-bus-connection-string")] string myQueueItem,
        ILogger log)
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

        log.LogInformation(
            $"ProcessOnMapChange :: GameName: '{onMapChange.GameName}', GameType: '{onMapChange.GameType}', MapName: '{onMapChange.MapName}'");
    }

    private async Task<GameServer> GetGameServer(ILogger log, string id)
    {
        var client = new RestClient(ApimBaseUrl);
        var request = new RestRequest("repository/GameServer");
        var accessToken = await RepositoryTokenProvider.GetRepositoryAccessToken();

        request.AddHeader("Ocp-Apim-Subscription-Key", ApimSubscriptionKey);
        request.AddHeader("Authorization", $"Bearer {accessToken}");
        request.AddParameter(new QueryParameter("id", id));

        var response = await client.ExecuteAsync(request);

        if (response.IsSuccessful)
            return JsonConvert.DeserializeObject<GameServer>(response.Content);
        if (response.StatusCode == HttpStatusCode.NotFound)
            return null;
        throw new Exception("Failed to execute 'repository/GameServer'");
    }

    private async Task CreateGameServer(ILogger log, GameServer gameServer)
    {
        var client = new RestClient(ApimBaseUrl);
        var request = new RestRequest("repository/GameServer", Method.Post);
        var accessToken = await RepositoryTokenProvider.GetRepositoryAccessToken();

        request.AddHeader("Ocp-Apim-Subscription-Key", ApimSubscriptionKey);
        request.AddHeader("Authorization", $"Bearer {accessToken}");
        request.AddJsonBody(gameServer);

        await client.ExecuteAsync(request);
    }
}