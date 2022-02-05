using XtremeIdiots.Portal.DataLib;

namespace XtremeIdiots.Portal.RepositoryApiClient.GameServersEventsApi;

public interface IGameServersEventsApiClient
{
    Task CreateGameServerEvent(string accessToken, string id, GameServerEvent gameServerEvent);
}