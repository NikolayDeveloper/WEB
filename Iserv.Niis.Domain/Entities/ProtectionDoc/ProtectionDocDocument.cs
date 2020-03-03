using Iserv.Niis.Domain.Abstract;

namespace Iserv.Niis.Domain.Entities.ProtectionDoc
{
    public class ProtectionDocDocument : Entity<int>
    {
        public int ProtectionDocId { get; set; }
        public Entities.ProtectionDoc.ProtectionDoc ProtectionDoc { get; set; }
        public int DocumentId { get; set; }
        public Document.Document Document { get; set; }
    }
}