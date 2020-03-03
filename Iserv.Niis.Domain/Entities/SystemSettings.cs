using Iserv.Niis.Domain.Abstract;
using Iserv.Niis.Domain.Enums;

namespace Iserv.Niis.Domain.Entities
{
    /// <summary>
    /// Системные константы
    /// </summary>
    public class SystemSettings: Entity<int>
    {
        /// <summary>
        /// Значение константы
        /// </summary>
        public string Value { get; set; }
        /// <summary>
        /// Тип константы
        /// </summary>
        public SettingType SettingType { get; set; }
    }
}
