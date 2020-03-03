using Iserv.Niis.Domain.Abstract;
using Iserv.Niis.Domain.Entities.Security;

namespace Iserv.Niis.Domain.Entities.Document
{
    public class DocumentExecutor : Entity<int>, IHaveConcurrencyToken
    {
        public int DocumentId { get; set; }
        public Document Document { get; set; }
        public int UserId { get; set; }
        public ApplicationUser User { get; set; }
    }
}