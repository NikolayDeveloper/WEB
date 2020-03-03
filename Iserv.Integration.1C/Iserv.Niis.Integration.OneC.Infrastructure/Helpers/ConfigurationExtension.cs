using System;
using Microsoft.Extensions.Configuration;

namespace Iserv.Niis.Integration.OneC.Infrastructure.Helpers
{
    /// <summary>
    /// Расширения для <see cref="IConfiguration"/>
    /// </summary>
    public static class ConfigurationExtension
    {
        /// <summary>
        /// Получает строку подключения к 1C.
        /// </summary>
        /// <param name="configuration">Представляет набор свойств конфигурации приложения ключ / значение.</param>
        /// <returns></returns>
        public static string GetOneCConnectionString(this IConfiguration configuration)
        {
            var configurationSection = configuration?.GetSection("1CConnection");

            if (configurationSection == null)
                throw new ArgumentNullException(nameof(configurationSection));

            var useServerMode = Convert.ToBoolean(configurationSection.GetSection("UseServerMode").Value);
            var userName = configurationSection.GetSection("UserName").Value;
            var password = configurationSection.GetSection("Password").Value;
            var pathToFile = configurationSection.GetSection("PathToFile").Value;
            var server = configurationSection.GetSection("Server").Value;
            var infoBaseName = configurationSection.GetSection("InfoBaseName").Value;

            var result = useServerMode
                ? $"Srvr=\"{server}\";Ref=\"{infoBaseName}\";Usr=\"{userName}\";Pwd=\"{password}\""
                : $"File=\"{pathToFile}\";Usr=\"{userName}\";Pwd=\"{password}\"";

            return result;
        }
    }
}
