using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Iserv.Niis.Domain.Entities.Request;
using Microsoft.EntityFrameworkCore;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Queries;

namespace Iserv.Niis.BusinessLogic.Requests
{
    public class GetRequestsByDocumentIdQuery: BaseQuery
    {
        public async Task<List<Request>> ExecuteAsync(int documentId)
        {
            var repo = Uow.GetRepository<Request>();
            var requests = await repo.AsQueryable()
                .Include(r => r.Division)
                .Include(r => r.CurrentWorkflow).ThenInclude(cw => cw.CurrentStage)
                .Where(r => r.Documents.Any(d => d.DocumentId == documentId))
                .ToListAsync();

            return requests;
        }
    }
}
