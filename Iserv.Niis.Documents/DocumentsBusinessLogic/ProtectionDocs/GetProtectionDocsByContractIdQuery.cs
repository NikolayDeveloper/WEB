using System.Collections.Generic;
using System.Linq;
using Iserv.Niis.Domain.Entities.ManyToManyMappingEntities;
using Iserv.Niis.Domain.Entities.ProtectionDoc;
using Microsoft.EntityFrameworkCore;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Queries;


namespace Iserv.Niis.Documents.DocumentsBusinessLogic.ProtectionDocs
{
    public class GetProtectionDocsByContractIdQuery : BaseQuery
    {
        public List<ProtectionDoc> Execute(int contractId)
        {
            var repo = Uow.GetRepository<ContractProtectionDocRelation>();

            return repo.AsQueryable()
                .Include(c => c.ProtectionDoc)
                .Where(c => c.ContractId == contractId)
                .Select(c => c.ProtectionDoc)
                .ToList();
        }
    }
}
