using XtremeIdiots.Portal.DataLib;

namespace XtremeIdiots.Portal.RepositoryApiClient.GameServersApi;

public interface IGameServersApiClient
{
    Task<List<GameServer>?> GetGameServers(string accessToken);
}