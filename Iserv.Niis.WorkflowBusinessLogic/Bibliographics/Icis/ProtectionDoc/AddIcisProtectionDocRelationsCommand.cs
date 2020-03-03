using System.Collections.Generic;
using Iserv.Niis.Domain.Entities.ProtectionDoc;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Commands;

namespace Iserv.Niis.WorkflowBusinessLogic.Bibliographics.Icis.ProtectionDoc
{
    public class AddIcisProtectionDocRelationsCommand : BaseCommand
    {
        public void Execute(int protectionDocId, List<int> icisIds)
        {
            var protectionDocRepository = Uow.GetRepository<Domain.Entities.ProtectionDoc.ProtectionDoc>();
            var protectionDoc = protectionDocRepository.GetById(protectionDocId);
            foreach (var id in icisIds)
            {
                protectionDoc.IcisProtectionDocs.Add(new ICISProtectionDoc { ProtectionDocId = protectionDocId, IcisId = id });
            }

            Uow.SaveChanges();
        }
    }
}
