using Iserv.Niis.Domain.Abstract;
using Iserv.Niis.Domain.Entities.Security;

namespace Iserv.Niis.Domain.Entities.Settings
{
    /// <summary>
    /// Пользовательские настройки.
    /// </summary>
    public class UserSetting : Entity<int>
    {
        /// <summary>
        /// Id пользователя.
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Представляет пользователя системы.
        /// </summary>
        public ApplicationUser User { get; set; }

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
