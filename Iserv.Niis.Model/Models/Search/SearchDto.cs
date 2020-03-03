using System;
using System.ComponentModel.DataAnnotations;
using Iserv.Niis.Domain.Enums;
using Iserv.Niis.Domain.Helpers;

namespace Iserv.Niis.Model.Models.Search
{
    public class SearchDto
    {
        public Owner.Type OwnerType { get; set; }
        public DocumentType? DocumentType { get; set; }
        public int Id { get; set; }
        [Display(Name = "Штрихкод")]
        public int Barcode { get; set; }
        [Display(Name = "№ документа")]
        public string Num { get; set; }
        [Display(Name = "Дата документа")]
        public DateTimeOffset? Date { get; set; }
        [Display(Name = "Описание")]
        public string Description { get; set; }
        [Display(Name = "ИИН/БИН контрагента")]
        public string Xin { get; set; }
        [Display(Name = "Контрагент")]
        public string Customer { get; set; }
        [Display(Name = "Адрес контрагента")]
        public string Address { get; set; }
        public int? CountryId { get; set; }
        [Display(Name = "Страна контрагента")]
        public string CountryNameRu { get; set; }
        public int? ReceiveTypeId { get; set; }
        [Display(Name = "Тип подачи")]
        public string ReceiveTypeNameRu { get; set; }
    }
}