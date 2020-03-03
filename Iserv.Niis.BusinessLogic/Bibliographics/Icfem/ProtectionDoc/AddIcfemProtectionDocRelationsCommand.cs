using System.Collections.Generic;
using System.Threading.Tasks;
using Iserv.Niis.Domain.Entities.ManyToManyMappingEntities;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Commands;

namespace Iserv.Niis.BusinessLogic.Bibliographics.Icfem.ProtectionDoc
{
    public class AddIcfemProtectionDocRelationsCommand: BaseCommand
    {
        public async Task ExecuteAsync(int protectionDocId, List<int> icfemIds)
        {
            var protectionDocRepository = Uow.GetRepository<Domain.Entities.ProtectionDoc.ProtectionDoc>();
            var protectionDoc = await protectionDocRepository.GetByIdAsync(protectionDocId);
            foreach (var id in icfemIds)
            {
                protectionDoc.Icfems.Add(
                    new DicIcfemProtectionDocRelation {DicIcfemId = id, ProtectionDocId = protectionDocId});
            }
            await Uow.SaveChangesAsync();
        }
    }
}
