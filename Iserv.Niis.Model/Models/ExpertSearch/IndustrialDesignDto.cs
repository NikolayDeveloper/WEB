using System;
using System.ComponentModel.DataAnnotations;

namespace Iserv.Niis.Model.Models.ExpertSearch
{
    public class IndustrialDesignDto : BaseExpertSearchDto
    {
        [Display(Name = "Изображение")]
        public string PreviewImage { get; set; }
        [Display(Name = "№ патента")]
        public string GosNumber { get; set; }
        [Display(Name = "Патентообладатель")]
        public string PatentOwner { get; set; }
        [Display(Name = "МКПО")]
        public string Icis { get; set; }
        [Display(Name = "Автор")]
        public string Author { get; set; }
        [Display(Name = "Существенные признаки")]
        public string Referat { get; set; }
        [Display(Name = "Дата досрочного прекращения")]
        public DateTimeOffset? EarlyTerminationDate { get; set; }
    }
}