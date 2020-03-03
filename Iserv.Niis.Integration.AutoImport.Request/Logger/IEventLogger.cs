using System.Diagnostics;

namespace Iserv.Niis.Integration.AutoImport.Request.Logger
{
    /// <summary>
    /// Логирование событий системы
    /// </summary>
    public interface IEventLogger
    {
        /// <summary>
        /// Запись лога события
        /// </summary>
        /// <param name="log">Текст лога</param>
        /// <param name="level">Уровень события</param>
        void WriteLog(string log, TraceLevel level = TraceLevel.Info);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="log"></param>
        void Write(string log);

        /// <summary>
        /// Записать лог в файл
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="log"></param>
        void WriteFileLog(string fileName, string log);

        /// <summary>
        /// Записать лог в файл
        /// </summary>
        /// <param name="log"></param>
        void WriteFileLog(string log);
    }
}