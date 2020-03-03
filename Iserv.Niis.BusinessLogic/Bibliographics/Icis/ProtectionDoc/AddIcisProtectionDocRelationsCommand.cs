using System.Collections.Generic;
using System.Threading.Tasks;
using Iserv.Niis.Domain.Entities.ProtectionDoc;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Commands;

namespace Iserv.Niis.BusinessLogic.Bibliographics.Icis.ProtectionDoc
{
    public class AddIcisProtectionDocRelationsCommand: BaseCommand
    {
        public async Task ExecuteAsync(int protectionDocId, List<int> icisIds)
        {
            var protectionDocRepository = Uow.GetRepository<Domain.Entities.ProtectionDoc.ProtectionDoc>();
            var protectionDoc = await protectionDocRepository.GetByIdAsync(protectionDocId);
            foreach (var id in icisIds)
            {
                protectionDoc.IcisProtectionDocs.Add(new ICISProtectionDoc{ProtectionDocId = protectionDocId, IcisId = id});
            }
            await Uow.SaveChangesAsync();
        }
    }
}
