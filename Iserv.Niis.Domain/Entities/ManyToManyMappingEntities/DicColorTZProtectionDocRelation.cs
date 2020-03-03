using Iserv.Niis.Domain.Entities.Dictionaries;

namespace Iserv.Niis.Domain.Entities.ManyToManyMappingEntities
{
    public class DicColorTZProtectionDocRelation
    {
        public int ColorTzId { get; set; }
        public DicColorTZ ColorTz { get; set; }
        public int ProtectionDocId { get; set; }
        public ProtectionDoc.ProtectionDoc ProtectionDoc { get; set; }
    }
}