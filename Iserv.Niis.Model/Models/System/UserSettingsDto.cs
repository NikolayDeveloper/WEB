namespace Iserv.Niis.Model.Models.System
{
    /// <summary>
    /// Пользовательские настройки.
    /// </summary>
    public class UserSettingsDto
    {
        /// <summary>
        /// Ключ к которому относятся настройки.
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// Значение настроек в формате JSON.
        /// </summary>
        public string Value { get; set; }
    }
}
