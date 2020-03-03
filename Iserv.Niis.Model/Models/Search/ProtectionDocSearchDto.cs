using System;
using System.ComponentModel.DataAnnotations;
using Iserv.Niis.Domain.Helpers;

namespace Iserv.Niis.Model.Models.Search
{
    public class ProtectionDocSearchDto
    {
        [Display(Name = "ID")]
        public int Id { get; set; }
        public int? statusId { get; set; }
        [Display(Name = "Статус ОД")]
        public string statusNameRu { get; set; }
        public int TypeId { get; set; }
        [Display(Name = "Тип ОД")]
        public string TypeNameRu { get; set; }
        public int? CurrentStageId { get; set; }
        [Display(Name = "Этап")]
        public string CurrentStageNameRu { get; set; }
        [Display(Name = "Дата этапа")]
        public DateTimeOffset? WorkflowDate { get; set; }
        [Display(Name = "Дата публикации")]
        public DateTimeOffset? PublicDate { get; set; }
        [Display(Name = "№ ОД")]
        public string GosNumber { get; set; }
        [Display(Name = "Дата ОД")]
        public DateTimeOffset? GosDate { get; set; }
        [Display(Name = "Наименование")]
        public string Name { get; set; }
        [Display(Name = "Срок действия ОД")]
        public DateTimeOffset? ValidDate { get; set; } //STZ17
        [Display(Name = "ИИН/БИН контрагента")]
        public string CustomerXin { get; set; }
        [Display(Name = "Контрагент")]
        public string CustomerNameRu { get; set; }
        [Display(Name = "Адрес контрагента")]
        public string CustomerAddress { get; set; }
        public int? CustomerCountryId { get; set; }
        [Display(Name = "Страна контрагента")]
        public string CustomerCountryNameRu { get; set; }
    }
}