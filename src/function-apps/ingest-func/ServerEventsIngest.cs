using System;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using XtremeIdiots.Portal.CommonLib.Events;
using XtremeIdiots.Portal.DataLib;
using XtremeIdiots.Portal.FuncHelpers.Providers;
using XtremeIdiots.Portal.RepositoryApiClient.GameServersApi;
using XtremeIdiots.Portal.RepositoryApiClient.GameServersEventsApi;

namespace XtremeIdiots.Portal.IngestFunc;

public class ServerEventsIngest
{
    private readonly IGameServersApiClient _gameServersApiClient;
    private readonly IGameServersEventsApiClient _gameServersEventsApiClient;
    private readonly ILogger _log;
    private readonly IRepositoryTokenProvider _repositoryTokenProvider;

    public ServerEventsIngest(ILogger log,
        IRepositoryTokenProvider repositoryTokenProvider,
        IGameServersApiClient gameServersApiClient, 
        IGameServersEventsApiClient gameServersEventsApiClient)
    {
        _log = log;
        _repositoryTokenProvider = repositoryTokenProvider;
        _gameServersApiClient = gameServersApiClient;
        _gameServersEventsApiClient = gameServersEventsApiClient;
    }

    [FunctionName("ProcessOnServerConnected")]
    public async Task ProcessOnServerConnected(
        [ServiceBusTrigger("server_connected_queue", Connection = "service-bus-connection-string")]
        string myQueueItem)
    {
        OnServerConnected onServerConnected;
        try
        {
            onServerConnected = JsonConvert.DeserializeObject<OnServerConnected>(myQueueItem);
        }
        catch (Exception ex)
        {
            _log.LogError(ex, "OnServerConnected was not in expected format");
            throw;
        }

        if (onServerConnected == null)
            throw new Exception("OnServerConnected event was null");

        if (string.IsNullOrWhiteSpace(onServerConnected.Id))
            throw new Exception("OnServerConnected event contained null or empty 'onServerConnected'");

        _log.LogInformation(
            $"OnServerConnected :: Id: '{onServerConnected.Id}', GameType: '{onServerConnected.GameType}'");

        var accessToken = await _repositoryTokenProvider.GetRepositoryAccessToken();
        var existingServer = await _gameServersApiClient.GetGameServer(accessToken, onServerConnected.Id);

        if (existingServer == null)
        {
            var gameServer = new GameServer
            {
                Id = onServerConnected.Id,
                GameType = onServerConnected.GameType
            };

            await _gameServersApiClient.CreateGameServer(accessToken, gameServer);
        }
    }

    [FunctionName("ProcessOnMapChange")]
    public async Task ProcessOnMapChange(
        [ServiceBusTrigger("map_change_queue", Connection = "service-bus-connection-string")]
        string myQueueItem)
    {
        OnMapChange onMapChange;
        try
        {
            onMapChange = JsonConvert.DeserializeObject<OnMapChange>(myQueueItem);
        }
        catch (Exception ex)
        {
            _log.LogError(ex, "OnMapChange was not in expected format");
            throw;
        }

        if (onMapChange == null)
            throw new Exception("OnMapChange event was null");

        if (string.IsNullOrWhiteSpace(onMapChange.ServerId))
            throw new Exception("OnMapChange event contained null or empty 'ServerId'");

        _log.LogInformation(
            $"ProcessOnMapChange :: GameName: '{onMapChange.GameName}', GameType: '{onMapChange.GameType}', MapName: '{onMapChange.MapName}'");

        var gameServerEvent = new GameServerEvent
        {
            GameServerId = onMapChange.ServerId,
            Timestamp = onMapChange.EventGeneratedUtc,
            EventType = "MapChange",
            EventData = JsonConvert.SerializeObject(onMapChange)
        };

        var accessToken = await _repositoryTokenProvider.GetRepositoryAccessToken();
        await _gameServersEventsApiClient.CreateGameServerEvent(accessToken, onMapChange.ServerId, gameServerEvent);
    }
}