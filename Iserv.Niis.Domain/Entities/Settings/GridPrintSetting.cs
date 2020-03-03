using System;
using Iserv.Niis.Domain.Abstract;
using Iserv.Niis.Domain.Entities.Security;

namespace Iserv.Niis.Domain.Entities.Settings
{
    /// <summary>
    /// Настройки Печати из таблиц
    /// </summary>
    [Obsolete("Не факт, что потребуется в новой системе")]
    public class GridPrintSetting : Entity<int>
    {
        public int UserId { get; set; }
        public ApplicationUser User { get; set; }
        public string GridName { get; set; }
        public string PrintItemId { get; set; }
        public string Text { get; set; }

        public int? TemplateDataFileId { get; set; }

        //public PrintField[] Fields { get; set; }
        public string PrintFields { get; set; }
    }
}