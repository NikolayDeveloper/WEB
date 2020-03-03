using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Iserv.Niis.Domain.Entities.ProtectionDoc;
using Microsoft.EntityFrameworkCore;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Queries;

namespace Iserv.Niis.BusinessLogic.ProtectionDocs
{
    public class GetProtectionDocsByDocumentIdQuery: BaseQuery
    {
        public async Task<List<ProtectionDoc>> ExecuteAsync(int documentId)
        {
            var repo = Uow.GetRepository<ProtectionDoc>();
            var protectionDocs = await repo.AsQueryable()
                .Where(r => r.Documents.Any(d => d.DocumentId == documentId))
                .ToListAsync();

            return protectionDocs;
        }
    }
}
