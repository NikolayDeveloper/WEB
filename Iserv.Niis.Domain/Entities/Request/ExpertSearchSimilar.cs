using Iserv.Niis.Domain.Abstract;
using Iserv.Niis.Domain.Helpers;
using System.ComponentModel.DataAnnotations;

namespace Iserv.Niis.Domain.Entities.Request
{
    public class ExpertSearchSimilar : Entity<int>
    {
        [Display(Name = "Id заявки")]
        public int RequestId { get; set; }
        public Request Request { get; set; }
        public Owner.Type OwnerType { get; set; }
        public int? SimilarRequestId { get; set; }
        public Request SimilarRequest { get; set; }
        public int? SimilarProtectionDocId { get; set; }
        public ProtectionDoc.ProtectionDoc SimilarProtectionDoc { get; set; }
        [Display(Name = "Процент схожести - картинка")]
        public int ImageSimilarity { get; set; }
        [Display(Name = "Процент схожести - фонетика")]
        public int PhonSimilarity { get; set; }
        [Display(Name = "Процент схожести - семантика")]
        public int SemSimilarity { get; set; }
        //[Display(Name = "Процент схожести - картинка")]
        //public string ImageSimilarity { get; set; }
        //[Display(Name = "Процент схожести - фонетика")]
        //public string PhonSimilarity { get; set; }
        //[Display(Name = "Процент схожести - семантика")]
        //public string SemSimilarity { get; set; }
        [Display(Name = "Категория ОД")]
        public string ProtectionDocCategory { get; set; }
        [Display(Name = "Формула ОД")]
        public string ProtectionDocFormula { get; set; }
    }
}