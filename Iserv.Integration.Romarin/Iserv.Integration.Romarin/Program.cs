using System;
using System.Configuration;
using Iserv.Niis.DataAccess.EntityFramework;
using Iserv.Niis.Integration.Romarin.BL.Ftp;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Iserv.Niis.Integration.Romarin.Host
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            try
            {
                var ftpW = new FtpClientWrapper(ConfigurationManager.AppSettings["host"]
                    , ConfigurationManager.AppSettings["user"]
                    , ConfigurationManager.AppSettings["password"]
                    , ConfigurationManager.AppSettings["path"]
                    , ConfigurationManager.AppSettings["ConnectionString"]
                    , Convert.ToInt32(ConfigurationManager.AppSettings["MainExecutorId"])
                    , ConfigurationManager.AppSettings["LogConnectionString"]);
                ftpW.ProcessFiles();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Console.ReadLine();
            }
        }
    }

    public class DesignTimeNiisWebContextFactory : IDesignTimeDbContextFactory<NiisWebContext>
    {
        public NiisWebContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<NiisWebContext>();

            var connectionString = ConfigurationManager.AppSettings["ConnectionString"];
            builder.UseSqlServer(connectionString);

            return new NiisWebContext(builder.Options);
        }
    }
}