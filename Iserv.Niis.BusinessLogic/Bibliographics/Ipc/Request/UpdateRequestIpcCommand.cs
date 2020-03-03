using Iserv.Niis.Domain.Entities.Request;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Commands;

namespace Iserv.Niis.BusinessLogic.Bibliographics.Ipc.Request
{
    public class UpdateRequestIpcCommand : BaseCommand
    {
        public void Execute(IPCRequest ipcRequest)
        {
            var repo = Uow.GetRepository<IPCRequest>();
            repo.Update(ipcRequest);
            Uow.SaveChanges();
        }
    }
}