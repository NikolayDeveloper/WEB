using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Iserv.Niis.Model.Models.ExpertSearch
{
    public class SimilarProtectionDocDto
    {
        /*
        [Display(Name = "Штрихкод")]
        public int Barcode { get; set; }
        */
        [Display(Name = "Статус")]
        public string StatusNameRu { get; set; }

        [Display(Name = "Изображение")]
        public string PreviewImage { get; set; }

        [Display(Name = "№ ОД")]
        public string GosNumber { get; set; }

        [Display(Name = "Дата ОД")]
        public DateTimeOffset? GosDate { get; set; }

        [Display(Name = "Рег. номер заявки")]
        public string RegNumber { get; set; }

        [Display(Name = "Дата подачи заявки")]
        public DateTimeOffset? RegDate { get; set; }

        [Display(Name = "Наименование на рус")]
        public string NameRu { get; set; }

        [Display(Name = "Наименование на каз")]
        public string NameKz { get; set; }

        [Display(Name = "Наименование на англ")]
        public string NameEn { get; set; }

        [Display(Name = "Заявитель")]
        public string Declarant { get; set; }

        [Display(Name = "Владелец")]
        public string OwnerName { get; set; }
        /*
        [Display(Name = "Патентный поверенный")]
        public string PatentAttorney { get; set; }
        */
        /*
        [Display(Name = "Доверенное лицо")]
        public string Confidant { get; set; }
        */
        /*
        [Display(Name = "Тип подачи заявки")]
        public string ReceiveTypeNameRu { get; set; }
        */
        [Display(Name = "МКТУ")]
        public List<string> Icgs { get; set; }

        [Display(Name = "МКИЭТЗ")]
        public string Icfems { get; set; }

        [Display(Name = "Цвета")]
        public string Colors { get; set; }
        /*
        [Display(Name = "Транслитерация")]
        public string Transliteration { get; set; }
        */
        [Display(Name = "Приоритетные данные")]
        public string PriorityData { get; set; }
        /*
        [Display(Name = "Номер бюллетеня")]
        public string NumberBulletin { get; set; }
        */
        /*
        [Display(Name = "Дата публикации")]
        public DateTimeOffset? PublicDate { get; set; }
        */
        [Display(Name = "Срок действия")]
        public DateTimeOffset? ValidDate { get; set; }

        [Display(Name = "Дата продления")]
        public DateTimeOffset? ExtensionDateTz { get; set; }

        [Display(Name = "Дискламация")]
        public string DisclaimerRu { get; set; }

        [Display(Name = "Дискламация каз")]
        public string DisclaimerKz { get; set; }

        [Display(Name = "Делопроизводство (Госреестр)")]
        public string Gosreestr { get; set; }

        #region 'ExpertSearchSimilar properties'

        public int Id { get; set; }

        public int ExpertSearchSimilarId { get; set; }

        public int ImageSimilarity { get; set; }

        public int PhonSimilarity { get; set; }

        public int SemSimilarity { get; set; }

        [Display(Name = "Примечание")]
        public string ProtectionDocCategory { get; set; }
        /*
        [Display(Name = "Формула ОД")]
        public string ProtectionDocFormula { get; set; }
        */
        #endregion 'ExpertSearchSimilar properties'
    }
}