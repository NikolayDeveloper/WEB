using Microsoft.Extensions.DependencyInjection;

namespace Iserv.Niis.Migration.Host
{
    class Program
    {
        static void Main(string[] args)
        {
            var serviceProvider = Configurations.ConfigurationDependencies();

            using (var serviceScope = serviceProvider.CreateScope())
            {
                var migrateAllData = serviceScope.ServiceProvider.GetRequiredService<MigrateAllData>();
                migrateAllData.StartMigrate();
            }
        }
    }
}
