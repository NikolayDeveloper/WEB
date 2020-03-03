using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Iserv.Niis.Domain.Entities.ProtectionDoc;
using Microsoft.EntityFrameworkCore;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Queries;

namespace Iserv.Niis.BusinessLogic.Bibliographics.Ipc.ProtectionDoc
{
    public class GetIpcByProtectionDocIdQuery: BaseQuery
    {
        public async Task<List<IPCProtectionDoc>> ExecuteAsync(int protectionDocId)
        {
            var repository = Uow.GetRepository<IPCProtectionDoc>();

            return await repository.AsQueryable()
                .Where(i => i.ProtectionDocId == protectionDocId)
                .ToListAsync();
        }
    }
}
