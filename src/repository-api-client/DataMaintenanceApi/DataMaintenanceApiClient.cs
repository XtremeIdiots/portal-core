using RestSharp;

namespace XtremeIdiots.Portal.RepositoryApiClient.DataMaintenanceApi;

public class DataMaintenanceApiClient : BaseApiClient, IDataMaintenanceApiClient
{
    public DataMaintenanceApiClient(string apimBaseUrl, string apimSubscriptionKey)
        : base(apimBaseUrl, apimSubscriptionKey)
    {
    }

    public async Task PruneChatMessages(string accessToken)
    {
        await ExecuteAsync(CreateRequest("repository/DataMaintenance/PruneChatMessages", Method.Delete, accessToken));
    }

    public async Task PruneGameServerEvents(string accessToken)
    {
        await ExecuteAsync(
            CreateRequest("repository/DataMaintenance/PruneGameServerEvents", Method.Delete, accessToken));
    }
}