using RestSharp;

namespace XtremeIdiots.Portal.RepositoryApiClient.DataMaintenanceApi;

public class DataMaintenanceApiClient : IDataMaintenanceApiClient
{
    private readonly string _apimBaseUrl;
    private readonly string _apimSubscriptionKey;

    public DataMaintenanceApiClient(string apimBaseUrl, string apimSubscriptionKey)
    {
        _apimBaseUrl = apimBaseUrl;
        _apimSubscriptionKey = apimSubscriptionKey;
    }

    public async Task PruneChatMessages(string accessToken)
    {
        var client = new RestClient(_apimBaseUrl);
        var request = new RestRequest("repository/DataMaintenance/PruneChatMessages", Method.Delete);

        request.AddHeader("Ocp-Apim-Subscription-Key", _apimSubscriptionKey);
        request.AddHeader("Authorization", $"Bearer {accessToken}");

        await client.ExecuteAsync(request);
    }
}