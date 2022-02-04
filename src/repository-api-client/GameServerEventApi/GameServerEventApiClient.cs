using RestSharp;
using XtremeIdiots.Portal.DataLib;

namespace XtremeIdiots.Portal.RepositoryApiClient.GameServerEventApi
{
    public class GameServerEventApiClient : IGameServerEventApiClient
    {
        private readonly string _apimBaseUrl;
        private readonly string _apimSubscriptionKey;

        public GameServerEventApiClient(string apimBaseUrl, string apimSubscriptionKey)
        {
            _apimBaseUrl = apimBaseUrl;
            _apimSubscriptionKey = apimSubscriptionKey;
        }

        public async Task CreateGameServerEvent(string accessToken, string id, GameServerEvent gameServerEvent)
        {
            var client = new RestClient(_apimBaseUrl);
            var request = new RestRequest($"repository/GameServer/{id}/event", Method.Post);

            request.AddHeader("Ocp-Apim-Subscription-Key", _apimSubscriptionKey);
            request.AddHeader("Authorization", $"Bearer {accessToken}");
            request.AddJsonBody(gameServerEvent);

            await client.ExecuteAsync(request);
        }
    }
}
