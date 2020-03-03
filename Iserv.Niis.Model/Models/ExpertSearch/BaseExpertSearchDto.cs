using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Iserv.Niis.Domain.Helpers;

namespace Iserv.Niis.Model.Models.ExpertSearch
{
    public class BaseExpertSearchDto
    {
        [Display(Name = "Тип объекта")]
        public Owner.Type OwnerType { get; set; }
        public ProtectionDocSearchStatus SearchStatus { get; set; }
        [Display(Name = "ID")]
        public int Id { get; set; }
        [Display(Name = "Штрихкод")]
        public int Barcode { get; set; }
        public int? RequestTypeId { get; set; }
        [Display(Name = "Тип заявки")]
        public string RequestTypeNameRu { get; set; }
        public int? StatusId { get; set; }
        public string StatusCode { get; set; }
        [Display(Name = "Статус")]
        public string StatusNameRu { get; set; }
        [Display(Name = "Дата ОД")]
        public DateTimeOffset? GosDate { get; set; }
        [Display(Name = "Рег. номер заявки")]
        public string RequestNum { get; set; }
        [Display(Name = "Дата подачи заявки")]
        public DateTimeOffset? RequestDate { get; set; }
        [Display(Name = "Наименование")]
        public string Name { get; set; }
        [Display(Name = "Наименование на рус")]
        public string NameRu { get; set; }
        [Display(Name = "Наименование на каз")]
        public string NameKz { get; set; }
        [Display(Name = "Наименование на англ")]
        public string NameEn { get; set; }
        [Display(Name = "Заявитель")]
        public string Declarant { get; set; }
        [Display(Name = "Патентный поверенный")]
        public string PatentAttorney { get; set; }
        [Display(Name = "Адрес для переписки")]
        public string AddressForCorrespondence { get; set; }
        [Display(Name = "Доверенное лицо")]
        public string Confidant { get; set; }
        public int? ReceiveTypeId { get; set; }
        [Display(Name = "Тип подачи заявки")]
        public string ReceiveTypeNameRu { get; set; }
        public string PriorityRegCountryNames { get; set; }
        public string PriorityRegNumbers { get; set; }
        public DateTimeOffset? ToPriorityDate { get; set; }
        public IEnumerable<DateTimeOffset?> PriorityDates { get; set; }
        [Display(Name = "Приоритетные данные")]
        public string PriorityData { get; set; }
        [Display(Name = "Номер бюллетеня")]
        public string NumberBulletin { get; set; }
        [Display(Name = "Дата публикации")]
        public DateTimeOffset? PublicDate { get; set; }
        [Display(Name = "Срок действия")]
        public DateTimeOffset? ValidDate { get; set; }

        [Display(Name = "Процент схожести - картинка")]
        public int ImageSimilarity { get; set; }
        [Display(Name = "Процент схожести - фонетика")]
        public int PhonSimilarity { get; set; }
        [Display(Name = "Процент схожести - семантика")]
        public int SemSimilarity { get; set; }

        public string ProtectionDocFormula { get; set; }
        public string ProtectionDocCategory { get; set; }
        public int ExpertSearchSimilarId { get; set; }
    }
}