using System.Collections.Generic;
using Iserv.Niis.Domain.Entities.ManyToManyMappingEntities;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Commands;

namespace Iserv.Niis.WorkflowBusinessLogic.Bibliographics.ColorTzs.ProtectionDoc
{
    public class AddColorTzProtectionDocRelationsCommand : BaseCommand
    {
        public void Execute(int protectionDocId, List<int> colorTzsIds)
        {
            var protectionDocRepository = Uow.GetRepository<Domain.Entities.ProtectionDoc.ProtectionDoc>();
            var protectionDoc = protectionDocRepository.GetById(protectionDocId);

            foreach (var colorTzId in colorTzsIds)
            {
                protectionDoc.ColorTzs.Add(
                    new DicColorTZProtectionDocRelation { ColorTzId = colorTzId, ProtectionDocId = protectionDocId });
            }

            Uow.SaveChanges();
        }
    }
}
