using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Iserv.Niis.Domain.Entities.ManyToManyMappingEntities;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Commands;

namespace Iserv.Niis.BusinessLogic.Bibliographics.ColorTzs.ProtectionDoc
{
    public class AddColorTzProtectionDocRelationsCommand: BaseCommand
    {
        public async Task ExecuteAsync(int protectionDocId, List<int> colorTzsIds)
        {
            var protectionDocRepository = Uow.GetRepository<Domain.Entities.ProtectionDoc.ProtectionDoc>();
            var protectionDoc = await protectionDocRepository.GetByIdAsync(protectionDocId);

            foreach (var colorTzId in colorTzsIds)
            {
                protectionDoc.ColorTzs.Add(
                    new DicColorTZProtectionDocRelation {ColorTzId = colorTzId, ProtectionDocId = protectionDocId});
            }
            await Uow.SaveChangesAsync();
        }
    }
}
