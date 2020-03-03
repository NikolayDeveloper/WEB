using System;
using System.ComponentModel.DataAnnotations;

namespace Iserv.Niis.Model.Models.ExpertSearch
{
    public class UsefulModelDto : BaseExpertSearchDto
    {
        [Display(Name = "№ патента")]
        public string GosNumber { get; set; }
        [Display(Name = "Патентообладатель")]
        public string PatentOwner { get; set; }
        [Display(Name = "МПК")]
        public string Ipcs { get; set; }
        [Display(Name = "Автор")]
        public string Author { get; set; }
        [Display(Name = "Дата досрочного прекращения")]
        public DateTimeOffset? EarlyTerminationDate { get; set; }
        [Display(Name = "Дата вхождения в национальную фазу")]
        public DateTimeOffset? TransferDate { get; set; }
        [Display(Name = "Формула")]
        public string Formula { get; set; }
        [Display(Name = "Реферат")]
        public string Referat { get; set; }
        [Display(Name = "Примечание")]
        public string Description { get; set; }
    }
}