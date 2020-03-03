using Iserv.Niis.Domain.Entities.ManyToManyMappingEntities;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Queries;
using System.Collections.Generic;
using System.Linq;

namespace Iserv.Niis.WorkflowBusinessLogic.Contracts
{
    public class GetProtectionDocIdsByContractIdQuery : BaseQuery
    {
        public List<int> Execute(int contractId)
        {
            var repo = Uow.GetRepository<ContractProtectionDocRelation>();

            var protectionDocIds = repo.AsQueryable()
                .Where(cr => cr.ContractId == contractId)
                .Select(cr => cr.ProtectionDoc.Id)
                .ToList();

            return protectionDocIds;
        }
    }
}
