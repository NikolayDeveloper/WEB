using Iserv.Niis.Domain.Abstract;
using Iserv.Niis.Domain.Entities.ManyToManyMappingEntities;
using Iserv.Niis.Domain.Entities.Request;

namespace Iserv.Niis.Domain.Entities.Contract
{
    public class ContractRequestICGSRequestRelation: Entity<int>
    {
        public int ContractRequestRelationId { get; set; }
        public ContractRequestRelation ContractRequestRelation { get; set; }

        public int ICGSRequestId { get; set; }
        public ICGSRequest ICGSRequest { get; set; }
        
        public string Description { get; set; }
    }
}