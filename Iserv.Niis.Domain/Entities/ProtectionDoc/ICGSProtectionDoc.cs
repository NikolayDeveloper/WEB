using Iserv.Niis.Domain.Abstract;
using Iserv.Niis.Domain.Entities.Dictionaries;

namespace Iserv.Niis.Domain.Entities.ProtectionDoc
{
    public class ICGSProtectionDoc : Entity<int>, IHaveConcurrencyToken
    {
        public int IcgsId { get; set; }
        public DicICGS Icgs { get; set; }

        public int ProtectionDocId { get; set; }
        public ProtectionDoc ProtectionDoc { get; set; }

        public bool? IsNegative { get; set; }
        public string Description { get; set; }
        public string DescriptionKz { get; set; }
        public string NegativeDescription { get; set; }
        public bool? IsNegativePartial { get; set; }
        public string ClaimedDescription { get; set; }
        public string ClaimedDescriptionEn { get; set; }
        public bool? IsRefused { get; set; }
        public bool? IsPartialRefused { get; set; }
        public string ReasonForPartialRefused { get; set; }
    }
}