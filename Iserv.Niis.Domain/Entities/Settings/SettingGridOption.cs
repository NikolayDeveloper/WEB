using System;
using Iserv.Niis.Domain.Abstract;
using Iserv.Niis.Domain.Entities.Security;

namespace Iserv.Niis.Domain.Entities.Settings
{
    /// <summary>
    /// класс для настройки представления таблицы
    /// </summary>
    [Obsolete("Не факт, что потребуется в новой системе")]
    public class SettingGridOption : Entity<int>
    {
        public int UserId { get; set; }
        public ApplicationUser User { get; set; }
        public string GridName { get; set; }
        public string Options { get; set; }
    }
}