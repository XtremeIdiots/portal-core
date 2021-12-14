using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using XtremeIdiots.Portal.DataLib;
using XtremeIdiots.Portal.RepositoryFunc;

[assembly: FunctionsStartup(typeof(Startup))]
namespace XtremeIdiots.Portal.RepositoryFunc
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            var config = builder.GetContext().Configuration;

            builder.Services.AddDbContext<PortalDbContext>(options => options.UseSqlServer(config["sql-connection-string"]));
        }
    }
}
