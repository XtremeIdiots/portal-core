using System.Threading.Tasks;

namespace XtremeIdiots.Portal.IngestFunc.Providers
{
    public interface IRepositoryTokenProvider
    {
        Task<string> GetRepositoryAccessToken();
    }
}