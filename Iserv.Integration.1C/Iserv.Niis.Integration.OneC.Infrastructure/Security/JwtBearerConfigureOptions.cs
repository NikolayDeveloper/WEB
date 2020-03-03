using System;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Iserv.Niis.Integration.OneC.Infrastructure.Security
{
    /// <summary>
    /// Настройки конфигурации.
    /// </summary>
    public static class JwtBearerConfigureOptions
    {
        /// <summary>
        /// Конфигурируем класс параметров предоставляет информацию, необходимую для управления поведением обработчика аутентификации Bearer.
        /// </summary>
        /// <param name="options">Класс параметров предоставляет информацию, необходимую для управления поведением обработчика аутентификации Bearer.</param>
        /// <param name="configuration">Представляет набор свойств конфигурации приложения ключ / значение.</param>
        public static void Configure(JwtBearerOptions options, IConfiguration configuration)
        {
            var jwtIssuerOptions = GetJwtIssuerOptions(configuration);

            options.RequireHttpsMetadata = false;
            options.TokenValidationParameters = new TokenValidationParameters
            {
                //Указывает, будет ли валидироваться издатель при валидации токена.
                ValidateIssuer = true,
                //Строка, представляющая издателя.
                ValidIssuer = jwtIssuerOptions.Issuer,

                //Будет ли валидироваться потребитель токена.
                ValidateAudience = false,

                //Будет ли валидироваться время существования.
                ValidateLifetime = true,

                //Установка ключа безопасности.
                IssuerSigningKey = GetSymmetricSecurityKey(jwtIssuerOptions.SigningKey),
                //Валидация ключа безопасности.
                ValidateIssuerSigningKey = true,
            };
        }

        /// <summary>
        /// Получает ключ безопасности.
        /// </summary>
        /// <param name="signingKey"><see cref="SecurityKey"/>, который должен использоваться для проверки подписи.</param>
        /// <returns>Ключ безопасности.</returns>
        public static SymmetricSecurityKey GetSymmetricSecurityKey(string signingKey)
        {
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(signingKey));
        }

        /// <summary>
        /// Получает настройки JWT.
        /// </summary>
        /// <param name="configuration">Представляет набор свойств конфигурации приложения ключ / значение.</param>
        /// <returns>Настройки издателя JWT токена.</returns>
        public static JwtIssuerOptions GetJwtIssuerOptions(IConfiguration configuration)
        {
            var jwtIssuerOptionsSection = configuration.GetSection(nameof(JwtIssuerOptions));

            return new JwtIssuerOptions
            {
                Issuer = jwtIssuerOptionsSection[nameof(JwtIssuerOptions.Issuer)],
                SigningKey = jwtIssuerOptionsSection[nameof(JwtIssuerOptions.SigningKey)],
                AccessKey = jwtIssuerOptionsSection[nameof(JwtIssuerOptions.AccessKey)],
                SecretKey = jwtIssuerOptionsSection[nameof(JwtIssuerOptions.SecretKey)],
                LifetimeInMinutes = Convert.ToInt32(jwtIssuerOptionsSection[nameof(JwtIssuerOptions.LifetimeInMinutes)])
            };
        }
    }
}
