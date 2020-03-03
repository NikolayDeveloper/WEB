using Iserv.Niis.Domain.Abstract;
using Iserv.Niis.Domain.Entities.ManyToManyMappingEntities;
using Iserv.Niis.Domain.Entities.ProtectionDoc;

namespace Iserv.Niis.Domain.Entities.Contract
{
    public class ContractProtectionDocICGSProtectionDocRelation : Entity<int>
    {
        public int ContractProtectionDocRelationId { get; set; }
        public ContractProtectionDocRelation ContractProtectionDocRelation { get; set; }

        public int ICGSProtectionDocId { get; set; }
        public ICGSProtectionDoc ICGSProtectionDoc { get; set; }

        public string Description { get; set; }
    }
}