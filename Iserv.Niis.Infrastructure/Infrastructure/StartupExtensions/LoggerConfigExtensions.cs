using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Serilog;

namespace Iserv.Niis.Infrastructure.StartupExtensions
{
    public static class LoggerConfigExtensions
    {
        /// <summary>
        /// Настройки логгера
        /// </summary>
        /// <param name="loggerFactory">Фабрика логгера</param>
        /// <param name="appLifetime"></param>
        /// <param name="configuration">Менеджер конфигураций</param>
        /// <returns></returns>
        public static ILoggerFactory SetupLogger(this ILoggerFactory loggerFactory, IApplicationLifetime appLifetime, IConfiguration configuration)
        {
            //Verbose = 0,
            //Debug = 1,
            //Information = 2,
            //Warning = 3,
            //Error = 4,
            //Fatal = 5
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .CreateLogger();

            appLifetime.ApplicationStopped.Register(Log.CloseAndFlush);

            loggerFactory.AddConsole(configuration.GetSection("Logging"));
            loggerFactory.AddDebug();
            loggerFactory.AddSerilog();

            return loggerFactory;
        }
    }
}
