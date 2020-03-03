using System;

namespace Iserv.Niis.Integration.AutoImport.RequestI.Extensions
{
    /// <summary>
    /// Расширения для <see cref="Environment"/>.
    /// </summary>
    public static class EnvironmentExtension
    {
        #region Возвращает true, если имя среды - Development.
        /// <summary>
        /// Возвращает true, если имя среды - Development.
        /// </summary>
        /// <returns>Возвращает true, если имя среды - Development.</returns>
        public static bool IsDevelopment()
        {
            return GetEnvironmentName().Contains("Development");
        }
        #endregion

        #region Возвращает true, если имя среды - Staging.
        /// <summary>
        /// Возвращает true, если имя среды - Staging.
        /// </summary>
        /// <returns>Возвращает true, если имя среды - Staging.</returns>
        public static bool IsStaging()
        {
            return GetEnvironmentName().Contains("Staging");
        }
        #endregion

        #region Возвращает true, если имя среды - Production.
        /// <summary>
        /// Возвращает true, если имя среды - Production.
        /// </summary>
        /// <returns>Возвращает true, если имя среды - Production.</returns>
        public static bool IsProduction()
        {
            return GetEnvironmentName().Contains("Production");
        }
        #endregion

        #region Возвращает описание среды, в которой хостируется приложение.
        /// <summary>
        /// Возвращает описание среды, в которой хостируется приложение.
        /// </summary>
        /// <returns>Описание среды, в которой хостируется приложение.</returns>
        public static string GetEnvironmentName()
        {
            return Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
        }
        #endregion
    }
}
