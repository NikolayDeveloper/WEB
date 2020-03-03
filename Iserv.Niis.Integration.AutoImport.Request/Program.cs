using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Iserv.Niis.Integration.AutoImport.Request.Services;
using Iserv.Niis.Integration.AutoImport.RequestI.Extensions;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.WindowsServices;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Iserv.Niis.Integration.AutoImport.Request
{
    public class Program
    {
        public static void Main(string[] args)
        {
            const string consoleArgument = "--console";

            var isService = !(Debugger.IsAttached || args.Contains(consoleArgument));

            var webHost = CreateWebHostBuilder(args.Where(arg => arg != consoleArgument).ToArray());

            if (isService)
            {
                webHost.RunAsService();
            }
            else
            {
                webHost.Run();
            }
        }

        public static IWebHost CreateWebHostBuilder(string[] args)
        {
            var configuration = GetConfiguration();

            var webHost = WebHost
                .CreateDefaultBuilder(args)
                .UseKestrel(KestrelOptions)
                .UseStartup<Startup>()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseIISIntegration()
                .UseConfiguration(configuration)
                .Build();

            var scope = webHost.Services.CreateScope();
            var autoImportRequestService = scope.ServiceProvider.GetService<IAutoImportRequestService>();
            autoImportRequestService.StartImport();
            
            
            return webHost;
        }

        #region Получаем настройки.
        /// <summary>
        /// Получаем настройки.
        /// </summary>
        /// <returns>Представляет набор свойств конфигурации приложения ключ/значение.</returns>
        private static IConfiguration GetConfiguration()
        {
            IConfigurationBuilder configurationBuilder = new ConfigurationBuilder();
            configurationBuilder
                .AddJsonFile($"appsettings.json", true, true)
                .AddJsonFile($"appsettings.{EnvironmentExtension.GetEnvironmentName()}.json", true, true);

            configurationBuilder.AddEnvironmentVariables();

            return configurationBuilder.Build();
        }
        #endregion

        #region Получить настройки для кроссплатформенного веб-сервер для ASP.NET Core.
        /// <summary>
        /// Получить настройки для кроссплатформенного веб-сервер для ASP.NET Core.
        /// </summary>
        /// <param name="options">Обеспечивает программную настройку специфичных для Kestrel функций.</param>
        private static void KestrelOptions(KestrelServerOptions options)
        {
            KestrelHttpOptions(options);
        }
        #endregion

        #region Получить настройки HTTP для кроссплатформенного веб-сервер для ASP.NET Core.
        /// <summary>
        /// Получить настройки HTTP для кроссплатформенного веб-сервер для ASP.NET Core.
        /// </summary>
        /// <param name="options">Обеспечивает программную настройку специфичных для Kestrel функций.</param>
        private static void KestrelHttpOptions(KestrelServerOptions options)
        {
            try
            {
                int httpPort = GetHttpPort();
                options.ListenAnyIP(httpPort);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
        #endregion

        #region Получаем HTTP порт на котором должен запуститься сервис.
        /// <summary>
        /// Получаем HTTP порт на котором должен запуститься сервис.
        /// </summary>
        /// <returns>Порт на котором должен запуститься сервис.</returns>
        private static int GetHttpPort()
        {
            return Convert.ToInt32(GetConfigurationValue("Host:Http:Port"));
        }
        #endregion
        
        #region Получаем значение из настроек.
        /// <summary>
        /// Получаем значение из настроек.
        /// </summary>
        /// <returns>Значение из настроек.</returns>
        private static string GetConfigurationValue(string key)
        {
            var configuration = GetConfiguration();

            var configurationSection = configuration?.GetSection(key);

            if (configurationSection == null)
                throw new ArgumentNullException(nameof(configurationSection));

            return configurationSection.Value;
        }
        #endregion
    }
}
