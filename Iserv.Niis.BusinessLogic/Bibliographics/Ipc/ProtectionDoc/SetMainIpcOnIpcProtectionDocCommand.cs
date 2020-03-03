using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Iserv.Niis.Domain.Entities.ProtectionDoc;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Commands;

namespace Iserv.Niis.BusinessLogic.Bibliographics.Ipc.ProtectionDoc
{
    public class SetMainIpcOnIpcProtectionDocCommand: BaseCommand
    {
        public void Execute(int protectionDocId, int mainIpcId)
        {
            var repo = Uow.GetRepository<IPCProtectionDoc>();
            var ipcProtectionDocs = repo
                .AsQueryable()
                .Where(i => i.ProtectionDocId == protectionDocId)
                .ToList();

            ipcProtectionDocs.ForEach(ipcProtectionDoc => ipcProtectionDoc.IsMain = false);
            repo.UpdateRange(ipcProtectionDocs);

            var mainIpcProtectionDoc = ipcProtectionDocs.FirstOrDefault(i => i.IpcId == mainIpcId);
            if (mainIpcProtectionDoc != null)
            {
                mainIpcProtectionDoc.IsMain = true;
                repo.Update(mainIpcProtectionDoc);
            }

            Uow.SaveChanges();
        }
    }
}
