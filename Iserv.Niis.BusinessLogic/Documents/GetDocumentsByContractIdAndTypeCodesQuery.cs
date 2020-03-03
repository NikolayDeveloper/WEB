using System.Collections.Generic;
using System.Linq;
using Iserv.Niis.Domain.Entities.Document;
using Microsoft.EntityFrameworkCore;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Queries;

namespace Iserv.Niis.BusinessLogic.Documents
{
    public class GetDocumentsByContractIdAndTypeCodesQuery : BaseQuery
    {
        public List<Document> Execute(int contractId, string[] typeCodes)
        {
            var documents = Uow.GetRepository<Document>()
                .AsQueryable()
                .Include(d => d.Type)
                .Include(d => d.Workflows).ThenInclude(cw => cw.CurrentStage)
                .Where(d => d.Contracts.Any(r => r.ContractId == contractId) && typeCodes.Contains(d.Type.Code))
                .ToList();
            return documents;
        }
    }
}