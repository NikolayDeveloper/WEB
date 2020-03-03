using Iserv.Niis.Domain.Entities.Request;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Commands;
using System.Linq;

namespace Iserv.Niis.BusinessLogic.Bibliographics.Ipc.Request
{
    public class SetMainIpcOnIpcRequestCommand : BaseCommand
    {
        public void Execute(int requestId, int mainIpcId)
        {
            var repo = Uow.GetRepository<IPCRequest>();
            var ipcRequests = repo
                .AsQueryable()
                .Where(i => i.RequestId == requestId)
                .ToList();

            ipcRequests.ForEach(ipcRequest => ipcRequest.IsMain = false);
            repo.UpdateRange(ipcRequests);

            var mainIpcRequest = ipcRequests.FirstOrDefault(i => i.IpcId == mainIpcId);
            if (mainIpcRequest != null)
            {
                mainIpcRequest.IsMain = true;
                repo.Update(mainIpcRequest);
            }

            Uow.SaveChanges();

        }
    }
}
