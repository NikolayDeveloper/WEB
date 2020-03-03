using System.Collections.Generic;
using Iserv.Niis.Domain.Entities.ManyToManyMappingEntities;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Commands;

namespace Iserv.Niis.WorkflowBusinessLogic.Bibliographics.Icfem.ProtectionDoc
{
    public class AddIcfemProtectionDocRelationsCommand : BaseCommand
    {
        public void Execute(int protectionDocId, List<int> icfemIds)
        {
            var protectionDocRepository = Uow.GetRepository<Domain.Entities.ProtectionDoc.ProtectionDoc>();
            var protectionDoc = protectionDocRepository.GetById(protectionDocId);
            foreach (var id in icfemIds)
            {
                protectionDoc.Icfems.Add(
                    new DicIcfemProtectionDocRelation { DicIcfemId = id, ProtectionDocId = protectionDocId });
            }

            Uow.SaveChanges();
        }
    }
}
