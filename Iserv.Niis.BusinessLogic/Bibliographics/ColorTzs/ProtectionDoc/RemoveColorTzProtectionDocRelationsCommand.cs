using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Iserv.Niis.Domain.Entities.ManyToManyMappingEntities;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Commands;

namespace Iserv.Niis.BusinessLogic.Bibliographics.ColorTzs.ProtectionDoc
{
    public class RemoveColorTzProtectionDocRelationsCommand: BaseCommand
    {
        public async Task ExecuteAsync(int protectionDocId, List<int> colorTzIds)
        {
            var repo = Uow.GetRepository<DicColorTZProtectionDocRelation>();

            foreach (var colorTzId in colorTzIds)
            {
                var colorTz = repo.AsQueryable()
                    .Where(i => i.ProtectionDocId == protectionDocId && i.ColorTzId == colorTzId);
                if (colorTz.Any())
                {
                    repo.DeleteRange(colorTz);
                }
            }
            await Uow.SaveChangesAsync();
        }
    }
}
