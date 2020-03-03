using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Iserv.Niis.Domain.Entities.ManyToManyMappingEntities;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Commands;

namespace Iserv.Niis.BusinessLogic.Bibliographics.Icfem.ProtectionDoc
{
    public class RemoveIcfemProtectionDocRelationsCommand: BaseCommand
    {
        public async Task ExecuteAsync(int protectionDocId, List<int> icfemIds)
        {
            var repo = Uow.GetRepository<DicIcfemProtectionDocRelation>();

            foreach (var icfemId in icfemIds)
            {
                var icfemProtectionDoc = repo.AsQueryable()
                    .Where(i => i.ProtectionDocId == protectionDocId && i.DicIcfemId == icfemId);
                if (icfemProtectionDoc.Any())
                {
                    repo.DeleteRange(icfemProtectionDoc);
                }
            }
            await Uow.SaveChangesAsync();
        }
    }
}
