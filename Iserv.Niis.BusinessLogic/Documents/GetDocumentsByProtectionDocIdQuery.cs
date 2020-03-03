using Iserv.Niis.Domain.Entities.Document;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Queries;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Iserv.Niis.BusinessLogic.Documents
{
    public class GetDocumentsByProtectionDocIdQuery : BaseQuery
    {
        public async Task<List<Document>> ExecuteAsync(int protectionDocId)
        {
            var repo = Uow.GetRepository<Document>();

            return await repo.AsQueryable()
                //.Include(d => d.CurrentWorkflows).ThenInclude(cw => cw.CurrentStage)
                //.Include(d => d.CurrentWorkflows).ThenInclude(cw => cw.CurrentUser)
                .Include(d => d.Workflows).ThenInclude(w => w.CurrentStage)
                .Include(d => d.Workflows).ThenInclude(w => w.CurrentUser)
                .Include(d => d.Type).Where(r =>
                    r.ProtectionDocs != null
                    && r.ProtectionDocs.Any(pd => pd.ProtectionDocId == protectionDocId)
                    && !r.IsDeleted)
                .ToListAsync();
        }
    }
}
