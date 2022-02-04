using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using XtremeIdiots.Portal.FuncHelpers.Providers;
using XtremeIdiots.Portal.RepositoryApiClient.DataMaintenanceApi;

namespace XtremeIdiots.Portal.RepositoryFunc;

public class DataMaintenance
{
    private readonly IDataMaintenanceApiClient _dataMaintenanceApiClient;
    private readonly ILogger _log;
    private readonly IRepositoryTokenProvider _repositoryTokenProvider;

    public DataMaintenance(
        ILogger log,
        IRepositoryTokenProvider repositoryTokenProvider,
        IDataMaintenanceApiClient dataMaintenanceApiClient)
    {
        _log = log;
        _repositoryTokenProvider = repositoryTokenProvider;
        _dataMaintenanceApiClient = dataMaintenanceApiClient;
    }

    [FunctionName("DataMaintenance")]
    public async Task RunDataMaintenance([TimerTrigger("0 0 * * * *")] TimerInfo myTimer)
    {
        _log.LogInformation("Performing Data Maintenance");

        var accessToken = await _repositoryTokenProvider.GetRepositoryAccessToken();
        await _dataMaintenanceApiClient.PruneChatMessages(accessToken);
        await _dataMaintenanceApiClient.PruneGameServerEvents(accessToken);
    }
}