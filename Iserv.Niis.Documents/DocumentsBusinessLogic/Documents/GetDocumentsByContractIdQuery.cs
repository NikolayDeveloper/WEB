using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Iserv.Niis.Domain.Entities.Document;
using Microsoft.EntityFrameworkCore;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Queries;

namespace Iserv.Niis.Documents.DocumentsBusinessLogic.Documents
{
    public class GetDocumentsByContractIdQuery: BaseQuery
    {
        public List<Document> Execute(int contractId)
        {
            var repo = Uow.GetRepository<Document>();
            var result = repo.AsQueryable()
                .Include(r => r.Type)
                .Include(r => r.Workflows).ThenInclude(w => w.CurrentUser)
                .Include(r => r.Workflows).ThenInclude(w => w.CurrentStage)
                .Where(d => d.Contracts.Any(rd => rd.ContractId == contractId));

            return result.ToList();
        }
    }
}
