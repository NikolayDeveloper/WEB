using System.Collections.Generic;
using Iserv.Niis.Domain.Entities.ProtectionDoc;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Commands;

namespace Iserv.Niis.WorkflowBusinessLogic.Bibliographics.ConventionInfos.ProtectionDoc
{
    public class AddProtectionDocConventionInfosRangeCommand : BaseCommand
    {
        public void Execute(int protectionDocId, List<ProtectionDocConventionInfo> infos)
        {
            var protectionDocRepository = Uow.GetRepository<Domain.Entities.ProtectionDoc.ProtectionDoc>();
            var protectionDoc = protectionDocRepository.GetById(protectionDocId);
            foreach (var info in infos)
            {
                protectionDoc.ProtectionDocConventionInfos.Add(info);
            }
            Uow.SaveChanges();
        }
    }
}
