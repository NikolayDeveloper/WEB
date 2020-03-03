using Iserv.Niis.Domain.Abstract;
using Iserv.Niis.Domain.Entities.Dictionaries;

namespace Iserv.Niis.Domain.Entities.Request
{
    public class ICISRequest : Entity<int>, IHaveConcurrencyToken
    {
        public int IcisId { get; set; }
        public DicICIS Icis { get; set; }
        public int RequestId { get; set; }
        public Request Request { get; set; }

        public string ImportedDate { get; set; }
    }
}