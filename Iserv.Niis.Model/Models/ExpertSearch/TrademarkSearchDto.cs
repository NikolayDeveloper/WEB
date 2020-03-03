using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Iserv.Niis.Model.Models.ExpertSearch
{
    public class TrademarkSearchDto : BaseExpertSearchDto
    {
        [Display(Name = "Изображение")]
        public string PreviewImage { get; set; }
        [Display(Name = "Рег. номер заявки")]
        public string RegNumber { get; set; }
        [Display(Name = "Дата подачи заявки")]
        public DateTimeOffset? RegDate { get; set; }
        [Display(Name = "№ ОД")]
        public string GosNumber { get; set; }
        [Display(Name = "Заявитель")]
        public string DeclarantName { get; set; }
        [Display(Name = "Владелец")]
        public string OwnerName { get; set; }
        [Display(Name = "МКТУ")]
        public List<string> Icgs { get; set; }
        [Display(Name = "Дискламация")]
        public string Disclamation { get; set; }
        [Display(Name = "Цвета")]
        public string Colors { get; set; }
        [Display(Name = "МКИЭТЗ")]
        public string Icfem { get; set; }
        //todo! делопроизводство
    }
}
