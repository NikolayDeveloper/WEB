using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;

namespace Iserv.Niis.Api.Infrastructure
{
    /// <summary>
    /// Расширения для интерфейса <see cref="IHostingEnvironment"/>
    /// </summary>
    public static class HostingEnvironmentExtensions
    {
        /// <summary>
        /// Возвращает является ли окружение предназначенным для разработки.
        /// </summary>
        /// <param name="hostingEnvironment">Предоставляет информацию о среде веб-хостинга, в которой запущено приложение.</param>
        /// <returns>Является ли окружение предназначенным для разработки.</returns>
        public static bool IsDevelopmentIserv(this IHostingEnvironment hostingEnvironment)
        {
            return hostingEnvironment.EnvironmentName.Contains("Development-iserv");
        }

        /// <summary>
        /// Возвращает является ли окружение предназначенным для разработки.
        /// </summary>
        /// <param name="hostingEnvironment">Предоставляет информацию о среде веб-хостинга, в которой запущено приложение.</param>
        /// <returns>Является ли окружение предназначенным для разработки.</returns>
        public static bool IsDevelopmentNiis(this IHostingEnvironment hostingEnvironment)
        {
            return hostingEnvironment.EnvironmentName.Contains("Development-niis");
        }

        /// <summary>
        /// Возвращает является ли окружение предназначенным для тестирования.
        /// </summary>
        /// <param name="hostingEnvironment">Предоставляет информацию о среде веб-хостинга, в которой запущено приложение.</param>
        /// <returns>Является ли окружение предназначенным для тестирования.</returns>
        public static bool IsStagingIserv(this IHostingEnvironment hostingEnvironment)
        {
            return hostingEnvironment.EnvironmentName.Contains("Staging-iserv");
        }

        /// <summary>
        /// Возвращает является ли окружение предназначенным для тестирования.
        /// </summary>
        /// <param name="hostingEnvironment">Предоставляет информацию о среде веб-хостинга, в которой запущено приложение.</param>
        /// <returns>Является ли окружение предназначенным для тестирования.</returns>
        public static bool IsStagingNiis(this IHostingEnvironment hostingEnvironment)
        {
            return hostingEnvironment.EnvironmentName.Contains("Staging-niis");
        }

        /// <summary>
        /// Возвращает является ли окружение предназначенным для релиза.
        /// </summary>
        /// <param name="hostingEnvironment">Предоставляет информацию о среде веб-хостинга, в которой запущено приложение.</param>
        /// <returns>Является ли окружение предназначенным для релиза.</returns>
        public static bool IsProductionIserv(this IHostingEnvironment hostingEnvironment)
        {
            return hostingEnvironment.EnvironmentName.Contains("Production-iserv");
        }

        /// <summary>
        /// Возвращает является ли окружение предназначенным для релиза.
        /// </summary>
        /// <param name="hostingEnvironment">Предоставляет информацию о среде веб-хостинга, в которой запущено приложение.</param>
        /// <returns>Является ли окружение предназначенным для релиза.</returns>
        public static bool IsProductionNiis(this IHostingEnvironment hostingEnvironment)
        {
            return hostingEnvironment.EnvironmentName.Contains("Production-niis");
        }
    }
}
