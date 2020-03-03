using System.Collections.Generic;
using System.Threading.Tasks;
using Iserv.Niis.Domain.Entities.Request;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Commands;

namespace Iserv.Niis.BusinessLogic.Bibliographics.Ipc.Request
{
    public class AddIpcRequestRelationsCommand: BaseCommand
    {
        public async Task ExecuteAsync(int requestId, List<int> ipcIds)
        {
            var requestRepository = Uow.GetRepository<Domain.Entities.Request.Request>();
            var request = await requestRepository.GetByIdAsync(requestId);
            foreach (var id in ipcIds)
            {
                request.IPCRequests.Add(new IPCRequest{RequestId = requestId, IpcId = id});
            }
            await Uow.SaveChangesAsync();
        }
    }
}
