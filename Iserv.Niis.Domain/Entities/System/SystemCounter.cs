using Iserv.Niis.Domain.Abstract;

namespace Iserv.Niis.Domain.Entities.System
{
    public class SystemCounter : Entity<int>, IHaveConcurrencyToken
    {
        public int Count { get; set; }
        public string Code { get; set; }
    }
}
