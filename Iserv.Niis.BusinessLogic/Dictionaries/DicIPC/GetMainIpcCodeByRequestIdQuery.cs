using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Queries;
using System.Linq;
using Iserv.Niis.Domain.Entities.Request;
using Microsoft.EntityFrameworkCore;
using Iserv.Niis.Model.Models.Dictionaries.Ipc;

namespace Iserv.Niis.BusinessLogic.Dictionaries.DicIPC
{
    public class GetMainIpcCodeByRequestIdQuery: BaseQuery
    {
        public IpcCodeDetail Execute(int requestId)
        {
            var repo = Uow.GetRepository<IPCRequest>();

            var ipc = repo
                .AsQueryable()
                .Include(ip => ip.Ipc)
                .FirstOrDefault(ip => ip.RequestId == requestId && ip.IsMain)?.Ipc;

            return new IpcCodeDetail (ipc?.Code ?? string.Empty);
        }
    }
}
