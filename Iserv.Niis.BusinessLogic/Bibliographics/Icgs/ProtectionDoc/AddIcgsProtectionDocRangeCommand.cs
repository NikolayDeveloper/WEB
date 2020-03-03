using System.Collections.Generic;
using System.Threading.Tasks;
using Iserv.Niis.Domain.Entities.ProtectionDoc;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Commands;

namespace Iserv.Niis.BusinessLogic.Bibliographics.Icgs.ProtectionDoc
{
    public class AddIcgsProtectionDocRangeCommand: BaseCommand
    {
        public async Task ExecuteAsync(int protectionDocId, List<ICGSProtectionDoc> icgs)
        {
            var protectionDocRepository = Uow.GetRepository<Domain.Entities.ProtectionDoc.ProtectionDoc>();
            var protectionDoc = await protectionDocRepository.GetByIdAsync(protectionDocId);
            foreach (var earlyReg in icgs)
            {
                protectionDoc.IcgsProtectionDocs.Add(earlyReg);
            }
            await Uow.SaveChangesAsync();
        }
    }
}
