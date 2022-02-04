using XtremeIdiots.Portal.DataLib;

namespace XtremeIdiots.Portal.RepositoryApiClient.GameServerEventApi;

public interface IGameServerEventApiClient
{
    Task CreateGameServerEvent(string accessToken, string id, GameServerEvent gameServerEvent);
}