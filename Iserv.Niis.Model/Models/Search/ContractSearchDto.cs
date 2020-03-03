using System;
using System.ComponentModel.DataAnnotations;

namespace Iserv.Niis.Model.Models.Search
{
    public class ContractSearchDto
    {
        [Display(Name = "ID")]
        public int Id { get; set; }
        public int? StatusId { get; set; }
        [Display(Name = "Статус договора")]
        public string StatusNameRu { get; set; }
        public int? ContractTypeId { get; set; }
        [Display(Name = "Вид договора")]
        public string ContractTypeNameRu { get; set; }
        public int? CategoryId { get; set; }
        [Display(Name = "Категория")]
        public string CategoryNameRu { get; set; }
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
        [Display(Name = "Рег. номер заявления")]
        public string ApplicationNum { get; set; }
        [Display(Name = "Дата подачи заявления")]
        public DateTimeOffset? DateCreate { get; set; }
        [Display(Name = "Рег. номер договора")]
        public string ContractNum { get; set; }
        [Display(Name = "Дата регистрации")]
        public DateTimeOffset? RegDate { get; set; }
        public int? ProtectionDocTypeId { get; set; }
        [Display(Name = "Тип ОД")]
        public string ProtectionDocTypeNameRu { get; set; }
        [Display(Name = "Предмет договора")]
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
        [Display(Name = "Место регистрации")]
        public string RegistrationPlace { get; set; }
        [Display(Name = "Срок действия договора")]
        public string ValidDate { get; set; }
    }
}