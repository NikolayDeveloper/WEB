using System.Collections.Generic;
using Iserv.Niis.Domain.Entities.ProtectionDoc;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Commands;

namespace Iserv.Niis.WorkflowBusinessLogic.Bibliographics.Icgs.ProtectionDoc
{
    public class AddIcgsProtectionDocRangeCommand : BaseCommand
    {
        public void Execute(int protectionDocId, List<ICGSProtectionDoc> icgs)
        {
            var protectionDocRepository = Uow.GetRepository<Domain.Entities.ProtectionDoc.ProtectionDoc>();
            var protectionDoc = protectionDocRepository.GetById(protectionDocId);
            foreach (var earlyReg in icgs)
            {
                protectionDoc.IcgsProtectionDocs.Add(earlyReg);
            }

            Uow.SaveChanges();
        }
    }
}
