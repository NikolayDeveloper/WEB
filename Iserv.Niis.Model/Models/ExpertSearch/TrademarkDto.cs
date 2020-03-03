using System;
using System.ComponentModel.DataAnnotations;

namespace Iserv.Niis.Model.Models.ExpertSearch
{
    public class TrademarkDto : BaseExpertSearchDto
    {
        [Display(Name = "Изображение")]
        public string PreviewImage { get; set; }
        [Display(Name = "№ ОД")]
        public string GosNumber { get; set; }
        [Display(Name = "Владелец")]
        public string Owner { get; set; }
        [Display(Name = "МКТУ")]
        public string Icgs { get; set; }
        [Display(Name = "МКИЭТЗ")]
        public string Icfems { get; set; }
        [Display(Name = "Транслитерация")]
        public string Transliteration { get; set; }
        [Display(Name = "Дата продления")]
        public DateTimeOffset? ExtensionDateTz { get; set; }
        [Display(Name = "Дискламация")]
        public string DisclaimerRu { get; set; }
        [Display(Name = "Дискламация каз")]
        public string DisclaimerKz { get; set; }

    }
}