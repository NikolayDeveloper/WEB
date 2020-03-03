using Microsoft.IdentityModel.Tokens;

namespace Iserv.Niis.Integration.OneC.Infrastructure.Security
{
    /// <summary>
    /// Настройки издателя JWT токена.
    /// </summary>
    public class JwtIssuerOptions
    {
        /// <summary>
        /// Получает или задает строку, представляющую действительного издателя, который будет использоваться для проверки по источнику токена.
        /// </summary>
        public string Issuer { get; set; }

        /// <summary>
        /// Получает или задает <see cref="SecurityKey"/>, который должен использоваться для проверки подписи.
        /// </summary>
        public string SigningKey { get; set; }

        /// <summary>
        /// Ключ доступа.
        /// </summary>
        public string AccessKey { get; set; }

        /// <summary>
        /// Пароль к ключу доступа.
        /// </summary>
        public string SecretKey { get; set; }

        /// <summary>
        /// Время истечения токена в минутах.
        /// </summary>
        public int LifetimeInMinutes { get; set; }
    }
}
