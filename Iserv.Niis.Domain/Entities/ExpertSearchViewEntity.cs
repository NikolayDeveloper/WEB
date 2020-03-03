using Iserv.Niis.Domain.Helpers;

namespace Iserv.Niis.Domain.Entities
{
    public class ExpertSearchViewEntity
    {
        public Owner.Type OwnerType { get; set; }
        public int ProtectionDocTypeId { get; set; }
        public int Id { get; set; }
        public int? RequestId { get; set; }
        public Request.Request Request { get; set; }
        public int? ProtectionDocId { get; set; }
        public ProtectionDoc.ProtectionDoc ProtectionDoc { get; set; }
    }
}