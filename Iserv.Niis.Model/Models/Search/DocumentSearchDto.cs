using System;
using System.ComponentModel.DataAnnotations;
using Iserv.Niis.Domain.Enums;
using Iserv.Niis.Domain.Helpers;

namespace Iserv.Niis.Model.Models.Search
{
    public class DocumentSearchDto
    {
        [Display(Name = "ID")]
        public int Id { get; set; }
        [Display(Name = "Штрих код")]
        public int Barcode { get; set; }
        [Display(Name = "Описание")]
        public string Description { get; set; }
        [Display(Name = "Тип документа")]
        public DocumentType DocumentType { get; set; }
        public int? TypeId { get; set; }
        public int? ReceiveTypeId { get; set; }
        [Display(Name = "Тип подачи")]
        public string ReceiveTypeNameRu { get; set; }
        [Display(Name = "Класс")]
        public string TypeNameRu { get; set; }
        [Display(Name = "Штат")]
        public string DepartmentNameRu { get; set; }
        public int? UserId { get; set; }
        [Display(Name = "Пользователь")]
        public string UserNameRu { get; set; }
        [Display(Name = "№ документа")]
        public string DocumentNum { get; set; }
        [Display(Name = "Дата документа")]
        public DateTimeOffset? DocumentDate { get; set; }
        [Display(Name = "Описание")]
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
        [Display(Name = "Исх. номер (контрагента)")]
        public string OutgoingNumber { get; set; }
        [Display(Name = "Дата документа (контрагента)")]
        public DateTimeOffset? SendingDate { get; set; }
    }
}