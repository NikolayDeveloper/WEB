using System.IO;
using Iserv.Niis.DataAccess.EntityFramework;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Iserv.Niis.Portal
{
    public class DesignTimeNiisWebContextFactory : IDesignTimeDbContextFactory<NiisWebContext>
    {
        public NiisWebContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var builder = new DbContextOptionsBuilder<NiisWebContext>();
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            builder.UseSqlServer(connectionString);

            return new NiisWebContext(builder.Options);
        }
    }
}