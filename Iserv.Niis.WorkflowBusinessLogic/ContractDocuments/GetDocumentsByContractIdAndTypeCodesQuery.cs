using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Queries;

namespace Iserv.Niis.WorkflowBusinessLogic.ContractDocuments
{
    public class GetDocumentsByContractIdAndTypeCodesQuery : BaseQuery
    {
        public List<Domain.Entities.Document.Document> Execute(int contractId, string[] typeCodes)
        {
            var documents = Uow.GetRepository<Domain.Entities.Document.Document>()
                .AsQueryable()
                .Include(d => d.Workflows).ThenInclude(cw => cw.CurrentStage)
                .Where(d => d.Contracts.Any(r => r.ContractId == contractId) && typeCodes.Contains(d.Type.Code))
                .ToList();
            return documents;
        }
    }
}