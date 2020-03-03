using System.Collections.Generic;
using System.Threading.Tasks;
using Iserv.Niis.Domain.Entities.ProtectionDoc;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Commands;

namespace Iserv.Niis.BusinessLogic.Bibliographics.Icgs.ProtectionDoc
{
    public class RemoveIcgsProtectionDocRangeCommand: BaseCommand
    {
        public async Task ExecuteAsync(int protectionDocId, List<ICGSProtectionDoc> icgsProtectionDocs)
        {
            var protectionDocRepository = Uow.GetRepository<Domain.Entities.ProtectionDoc.ProtectionDoc>();
            var protectionDoc = await protectionDocRepository.GetByIdAsync(protectionDocId);
            foreach (var icgsProtectionDoc in icgsProtectionDocs)
            {
                protectionDoc.IcgsProtectionDocs.Remove(icgsProtectionDoc);
            }
            await Uow.SaveChangesAsync();
        }
    }
}
