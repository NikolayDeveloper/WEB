using Iserv.Niis.Domain.Entities.Dictionaries;
using Iserv.Niis.Domain.Entities.Security;

namespace Iserv.Niis.Domain.Entities.ManyToManyMappingEntities
{
    public class RoleProtectionDocTypeRelation
    {
        public int RoleId { get; set; }
        public ApplicationRole Role { get; set; }
        public int ProtectionDocTypeId { get; set; }
        public DicProtectionDocType ProtectionDocType { get; set; }
    }
}