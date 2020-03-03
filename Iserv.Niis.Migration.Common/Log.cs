using System;
using NLog;

namespace Iserv.Niis.Migration.Common
{
    public static class Log
    {
        private static Logger _logger;

        static Log()
        {
            _logger = LogManager.GetLogger(nameof(Log));
        }

        public static void LogDebug(string message) => _logger.Debug(message);

        public static void LogInfo(string message) => _logger.Info(message);

        public static void LogError(Exception ex)
        {
            _logger.Error(ex);
        }

        public static void LogError(string message) => _logger.Error(message);
    }
}
