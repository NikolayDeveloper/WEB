using Iserv.Niis.Domain.Abstract;
using Iserv.Niis.Domain.Entities.Dictionaries;

namespace Iserv.Niis.Domain.Entities.ProtectionDoc
{
    public class ICISProtectionDoc : Entity<int>, IHaveConcurrencyToken
    {
        public int IcisId { get; set; }
        public DicICIS Icis { get; set; }
        public int ProtectionDocId { get; set; }
        public ProtectionDoc ProtectionDoc { get; set; }

        public string ImportedDate { get; set; }
    }
}