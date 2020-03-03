using System.Collections.Generic;
using Iserv.Niis.Domain.Entities.ProtectionDoc;
using Iserv.Niis.Domain.Entities.Request;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Commands;

namespace Iserv.Niis.WorkflowBusinessLogic.Bibliographics.Ipc.ProtectionDoc
{
    public class AddIpcProtectionDocRelationsCommand: BaseCommand
    {
        public void Execute(int protectionDocId, List<IPCRequest> ipcRequests)
        {
            var protectionDocRepository = Uow.GetRepository<Domain.Entities.ProtectionDoc.ProtectionDoc>();
            var protectionDoc = protectionDocRepository.GetById(protectionDocId);
            foreach (var ipcRequest in ipcRequests)
            {
                protectionDoc.IpcProtectionDocs.Add(
                    new IPCProtectionDoc {ProtectionDocId = protectionDocId, IpcId = ipcRequest.IpcId, IsMain = ipcRequest.IsMain});
            }

            Uow.SaveChangesAsync();
        }
    }
}
