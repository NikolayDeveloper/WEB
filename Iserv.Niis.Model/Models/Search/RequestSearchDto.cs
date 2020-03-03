using System;
using System.ComponentModel.DataAnnotations;
using Iserv.Niis.Domain.Helpers;

namespace Iserv.Niis.Model.Models.Search
{
    public class RequestSearchDto
    {
        [Display(Name = "ID")]
        public int Id { get; set; }
        public int? StatusId { get; set; }
        [Display(Name = "Статус заявки")]
        public string StatusNameRu { get; set; }
        public int? ProtectionDocTypeId { get; set; }
        [Display(Name = "Тип ОД")]
        public string ProtectionDocTypeNameRu { get; set; }
        public int? RequestTypeId { get; set; }
        [Display(Name = "Тип заявки")]
        public string RequestTypeNameRu { get; set; }
        public int? CurrentStageId { get; set; }
        [Display(Name = "Этап")]
        public string CurrentStageNameRu { get; set; }
        [Display(Name = "Дата этапа")]
        public DateTimeOffset? WorkflowDate { get; set; }
        [Display(Name = "Штат")]
        public string DepartmentNameRu { get; set; }
        public int? UserId { get; set; }
        [Display(Name = "Пользователь")]
        public string UserNameRu { get; set; }
        [Display(Name = "Рег. номер")]
        public string RequestNum { get; set; }
        [Display(Name = "Баркод")]
        public int Barcode { get; set; }
        [Display(Name = "Входящий номер")]
        public string IncomingNumber { get; set; }
        [Display(Name = "Дата подачи")]
        public DateTimeOffset? RequestDate { get; set; }
        [Display(Name = "Наименование")]
        public string Name { get; set; }
        [Display(Name = "ИИН/БИН контрагента")]
        public string CustomerXin { get; set; }
        [Display(Name = "Контрагент")]
        public string CustomerNameRu { get; set; }
        [Display(Name = "Адрес контрагента")]
        public string CustomerAddress { get; set; }
        public int? CustomerCountryId { get; set; }
        [Display(Name = "Страна контрагента")]
        public string CustomerCountryNameRu { get; set; }
        public int? ReceiveTypeId { get; set; }
        [Display(Name = "Тип подачи")]
        public string ReceiveTypeNameRu { get; set; }
    }
}