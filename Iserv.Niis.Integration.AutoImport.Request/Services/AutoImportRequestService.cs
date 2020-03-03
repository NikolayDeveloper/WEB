using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Iserv.Niis.Integration.AutoImport.Request.Logger;
using Iserv.Niis.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using Serilog;

namespace Iserv.Niis.Integration.AutoImport.Request.Services
{
    public class AutoImportRequestService : IAutoImportRequestService
    {
        private readonly IImportRequestHelper _importRequestHelper;
        private readonly IConfiguration _configuration;
        private readonly IEventLogger _eventLogger;

        public AutoImportRequestService(
            IConfiguration configuration, 
            IEventLogger eventLogger,
            IImportRequestHelper importRequestHelper
            )
        {
            _configuration = configuration;
            _eventLogger = eventLogger;
            _importRequestHelper = importRequestHelper;
        }

        public void StartImport()
        {
            _eventLogger.WriteLog($"Service started. Period {GetConfigurationValue("CheckPeriodInMinutes")} minutes");

#pragma warning disable 4014
            ThreadMethod();
#pragma warning restore 4014
        }

        private async Task ThreadMethod()
        {
            while (true)
            {
                await Cycle();
                Thread.Sleep(TimeSpan.FromMinutes(double.Parse(GetConfigurationValue("CheckPeriodInMinutes"))));
            }
            // ReSharper disable once FunctionNeverReturns
        }

        private async Task Cycle()
        {
            _eventLogger.WriteLog("Start Cycle");
            try
            {
                var date = DateTime.Now.Date;
                var configDateString = GetConfigurationValue("DateForImport");
                if (!string.IsNullOrEmpty(configDateString) && DateTime.TryParse(configDateString, out var configDateForImport))
                    date = configDateForImport;

                var result = await _importRequestHelper.ImportRequestByDate(date);
                _eventLogger.WriteLog(result != null && result.Any()
                    ? $"Requests whas imported with Id: {string.Join(", ", result)} for date {date.Date:dd-MM-yyyy}"
                    : $"Cycle result whas empty for date {date.Date:dd-MM-yyyy}");
            }
            catch (Exception ex)
            {
                _eventLogger.WriteLog("Error " + ex.Message, TraceLevel.Error);
                Log.Error(ex, ex.Message);
            }
            _eventLogger.WriteLog("End Cycle");
        }

        #region Получаем значение из настроек.
        /// <summary>
        /// Получаем значение из настроек.
        /// </summary>
        /// <returns>Значение из настроек.</returns>
        private string GetConfigurationValue(string key)
        {
            var configuration = _configuration;

            var configurationSection = configuration?.GetSection(key);

            if (configurationSection == null)
                throw new ArgumentNullException(nameof(configurationSection));

            return configurationSection.Value;
        }
        #endregion
    }
}