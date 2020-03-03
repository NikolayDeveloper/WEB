using System.Collections.Generic;
using System.Threading.Tasks;
using Iserv.Niis.Domain.Entities.ProtectionDoc;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Commands;

namespace Iserv.Niis.BusinessLogic.Bibliographics.ConventionInfos.ProtectionDoc
{
    public class AddProtectionDocConventionInfosRangeCommand: BaseCommand
    {
        public async Task ExecuteAsync(int protectionDocId, List<ProtectionDocConventionInfo> infos)
        {
            var protectionDocRepository = Uow.GetRepository<Domain.Entities.ProtectionDoc.ProtectionDoc>();
            var protectionDoc = await protectionDocRepository.GetByIdAsync(protectionDocId);
            foreach (var info in infos)
            {
                protectionDoc.ProtectionDocConventionInfos.Add(info);
            }
            await Uow.SaveChangesAsync();
        }
    }
}
