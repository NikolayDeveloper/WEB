using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Iserv.Niis.Domain.Entities.ProtectionDoc;
using Microsoft.EntityFrameworkCore;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Commands;

namespace Iserv.Niis.BusinessLogic.Bibliographics.Icis.ProtectionDoc
{
    public class RemoveIcisProtectionDocRelationsCommand: BaseCommand
    {
        public async Task ExecuteAsync(int protectionDocId, List<int> icisIds)
        {
            var icisRepository = Uow.GetRepository<ICISProtectionDoc>();

            foreach (var icisId in icisIds)
            {
                var icisProtectionDoc = icisRepository.AsQueryable()
                    .Where(i => i.ProtectionDocId == protectionDocId && i.IcisId == icisId);
                if (icisProtectionDoc.Any())
                {
                    icisRepository.DeleteRange(icisProtectionDoc);
                }
            }
            await Uow.SaveChangesAsync();
        }
    }
}
