using Iserv.Niis.Domain.Entities.Dictionaries;

namespace Iserv.Niis.Domain.Entities.ManyToManyMappingEntities
{
    public class DicIcfemProtectionDocRelation
    {
        public int DicIcfemId { get; set; }
        public DicICFEM DicIcfem { get; set; }
        public int ProtectionDocId { get; set; }
        public ProtectionDoc.ProtectionDoc ProtectionDoc { get; set; }
    }
}