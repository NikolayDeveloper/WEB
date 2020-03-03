using System.Collections.Generic;
using System.Threading.Tasks;
using Iserv.Niis.Domain.Entities.Request;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Commands;

namespace Iserv.Niis.BusinessLogic.Bibliographics.Ipc.Request
{
    public class DeleteRangeIpcRequestCommand : BaseCommand
    {
        public async Task ExecuteAsync(List<IPCRequest> ipcRequests)
        {
            var ipcRequestRepo = Uow.GetRepository<IPCRequest>();
            ipcRequestRepo.DeleteRange(ipcRequests);
            await Uow.SaveChangesAsync();
        }
    }
}