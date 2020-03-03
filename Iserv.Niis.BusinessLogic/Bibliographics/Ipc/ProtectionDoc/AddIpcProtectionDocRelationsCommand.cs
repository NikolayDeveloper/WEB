using System.Collections.Generic;
using System.Threading.Tasks;
using Iserv.Niis.Domain.Entities.ProtectionDoc;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Commands;

namespace Iserv.Niis.BusinessLogic.Bibliographics.Ipc.ProtectionDoc
{
    public class AddIpcProtectionDocRelationsCommand: BaseCommand
    {
        public async Task ExecuteAsync(int protectionDocId, List<int> ipcIds)
        {
            var protectionDocRepository = Uow.GetRepository<Domain.Entities.ProtectionDoc.ProtectionDoc>();
            var protectionDoc = await protectionDocRepository.GetByIdAsync(protectionDocId);
            foreach (var id in ipcIds)
            {
                protectionDoc.IpcProtectionDocs.Add(
                    new IPCProtectionDoc {ProtectionDocId = protectionDocId, IpcId = id});
            }
            await Uow.SaveChangesAsync();
        }
    }
}
