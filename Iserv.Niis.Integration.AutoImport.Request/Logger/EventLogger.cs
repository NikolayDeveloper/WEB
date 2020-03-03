using System;
using System.Diagnostics;
using System.IO;

namespace Iserv.Niis.Integration.AutoImport.Request.Logger
{
    /// <summary>
    /// Логирование событий системы
    /// </summary>
    public class EventLogger : IEventLogger
    {
        private readonly object _lockerFile = new object();

        /// <summary>
        /// Запись лога события
        /// </summary>
        /// <param name="log">Текст лога</param>
        /// <param name="level">Уровень события</param>
        public void WriteLog(string log, TraceLevel level = TraceLevel.Info)
        {
            var logMessage = $"{DateTime.Now}-{level}-{log}";

            WriteFileLog("Events", logMessage);
        }

        /// <summary>
        /// Запись лога события
        /// </summary>
        /// <param name="log">Текст лога</param>
        public void Write(string log)
        {
            Console.Write(log);
        }

        public void WriteFileLog(string fileName, string logStr)
        {
            lock (_lockerFile)
            {
                var logFilePath = "C:\\Logs\\AutoImportRequest\\";
                logFilePath = logFilePath + "Log-" + fileName + "-" + DateTime.Today.ToString("MM-dd-yyyy") + "." +"txt";
                var logFileInfo = new FileInfo(logFilePath);
                var logDirInfo = new DirectoryInfo(logFileInfo.DirectoryName ?? throw new InvalidOperationException());
                if (!logDirInfo.Exists) logDirInfo.Create();

                var fileStream = !logFileInfo.Exists ? logFileInfo.Create() : new FileStream(logFilePath, FileMode.Append);

                var log = new StreamWriter(fileStream);
                log.WriteLine(logStr);
                log.Close();
                log.Dispose();
            }
        }

        public void WriteFileLog(string logStr)
        {
            WriteFileLog("Full", logStr);
        }
    }
}