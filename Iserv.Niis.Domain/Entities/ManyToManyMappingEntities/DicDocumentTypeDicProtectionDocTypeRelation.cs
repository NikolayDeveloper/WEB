using Iserv.Niis.Domain.Entities.Dictionaries;

namespace Iserv.Niis.Domain.Entities.ManyToManyMappingEntities
{
    public class DicDocumentTypeDicProtectionDocTypeRelation
    {
        public int DicDocumentTypeId { get; set; }
        public DicDocumentType DicDocumentType { get; set; }
        public int DicProtectionDocTypeId { get; set; }
        public DicProtectionDocType DicProtectionDocType { get; set; }
    }
}
