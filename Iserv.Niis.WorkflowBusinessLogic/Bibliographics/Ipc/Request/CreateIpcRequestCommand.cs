using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Iserv.Niis.Domain.Entities.Request;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Commands;

namespace Iserv.Niis.WorkflowBusinessLogic.Bibliographics.Ipc.Request
{
    public class CreateIpcRequestCommand: BaseCommand
    {
        public int Execute(IPCRequest ipc)
        {
            var repo = Uow.GetRepository<IPCRequest>();
            repo.Create(ipc);
            Uow.SaveChanges();
            return ipc.Id;
        }
    }
}
