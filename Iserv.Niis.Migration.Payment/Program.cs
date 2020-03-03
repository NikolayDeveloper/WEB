using Microsoft.Extensions.DependencyInjection;
using System;

namespace Iserv.Niis.Migration.Payment
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var serviceProvider = Configurations.ConfigurationDependencies();

            using (var serviceScope = serviceProvider.CreateScope())
            {
                var migratePayment = serviceScope.ServiceProvider.GetRequiredService<MigratePayments>();
                migratePayment.Migrate();
                Console.ReadLine();
            }
        }
    }
}
