using Iserv.Niis.Domain.Entities.Request;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Commands;

namespace Iserv.Niis.BusinessLogic.Requests
{
    public class CreateRequestIpcCommand : BaseCommand
    {
        public int Execute(IPCRequest ipcRequest)
        {
            var repo = Uow.GetRepository<IPCRequest>();
            repo.Create(ipcRequest);
            Uow.SaveChanges();
            return ipcRequest.Id;
        }
    }
}