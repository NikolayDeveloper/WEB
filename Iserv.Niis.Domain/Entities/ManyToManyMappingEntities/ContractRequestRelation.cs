using System.Collections.Generic;
using Iserv.Niis.Domain.Abstract;
using Iserv.Niis.Domain.Entities.Contract;
using Iserv.Niis.Domain.Entities.Request;

namespace Iserv.Niis.Domain.Entities.ManyToManyMappingEntities
{
    public class ContractRequestRelation: Entity<int>
    {
        public int ContractId { get; set; }
        public Contract.Contract Contract { get; set; }
        public int RequestId { get; set; }
        public Request.Request Request { get; set; }
        
        public ICollection<ContractRequestICGSRequestRelation> ContractRequestICGSRequests { get; set; }
    }
}