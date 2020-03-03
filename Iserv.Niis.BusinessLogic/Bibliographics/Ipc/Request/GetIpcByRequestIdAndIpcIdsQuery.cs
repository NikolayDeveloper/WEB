using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Iserv.Niis.Domain.Entities.Request;
using Microsoft.EntityFrameworkCore;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Queries;

namespace Iserv.Niis.BusinessLogic.Bibliographics.Ipc.Request
{
    public class GetIpcByRequestIdAndIpcIdsQuery : BaseQuery
    {
        public async Task<List<IPCRequest>> ExecuteAsync(int requestId, List<int> ipcIds)
        {
            var ipcRequestRepo = Uow.GetRepository<IPCRequest>();
            return await ipcRequestRepo
                .AsQueryable()
                .Where(ip => ip.RequestId == requestId && ipcIds.Contains(ip.IpcId))
                .ToListAsync();
        }
    }
}