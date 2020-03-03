namespace Iserv.Niis.Integration.OneC.Model
{
    /// <summary>
    /// Модель для получения токена.
    /// </summary>
    public class TokenDto
    {
        /// <summary>
        /// Ключ доступа.
        /// </summary>
        public string AccessKey { get; set; }

        /// <summary>
        /// Пароль к ключу доступа.
        /// </summary>
        public string SecretKey { get; set; }
    }
}
