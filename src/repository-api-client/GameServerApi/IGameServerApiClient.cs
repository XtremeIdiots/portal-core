using XtremeIdiots.Portal.DataLib;

namespace XtremeIdiots.Portal.RepositoryApiClient.GameServerApi;

public interface IGameServerApiClient
{
    Task<GameServer?> GetGameServer(string accessToken, string id);
    Task CreateGameServer(string accessToken, GameServer gameServer);
    Task UpdateGameServer(string accessToken, GameServer gameServer);
}