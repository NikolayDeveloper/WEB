using System.Configuration;
using Iserv.Niis.ExternalServices.DataAccess.EntityFramework;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Iserv.Niis.ExternalServices.Host
{
    public class DesignTimeNiisIntegrationContextFactory : IDesignTimeDbContextFactory<NiisIntegrationContext>
    {
        public NiisIntegrationContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<NiisIntegrationContext>();

            var connectionString = ConfigurationManager.AppSettings[Constants.AppSettings.EgovConfiguration.ConStringNiisIntegration];

            builder.UseSqlServer(connectionString);

            return new NiisIntegrationContext(builder.Options);
        }
    }
}