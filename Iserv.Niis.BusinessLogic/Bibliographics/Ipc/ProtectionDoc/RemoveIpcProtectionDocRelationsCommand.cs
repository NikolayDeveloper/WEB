using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Iserv.Niis.Domain.Entities.ProtectionDoc;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Commands;

namespace Iserv.Niis.BusinessLogic.Bibliographics.Ipc.ProtectionDoc
{
    public class RemoveIpcProtectionDocRelationsCommand: BaseCommand
    {
        public async Task ExecuteAsync(int protectionDocId, List<int> ipcIds)
        {
            var repo = Uow.GetRepository<IPCProtectionDoc>();

            foreach (var ipcId in ipcIds)
            {
                var ipcProtectionDocs = repo.AsQueryable()
                    .Where(i => i.ProtectionDocId == protectionDocId && i.IpcId == ipcId);
                if (ipcProtectionDocs.Any())
                {
                    repo.DeleteRange(ipcProtectionDocs);
                }
            }
            await Uow.SaveChangesAsync();
        }
    }
}
