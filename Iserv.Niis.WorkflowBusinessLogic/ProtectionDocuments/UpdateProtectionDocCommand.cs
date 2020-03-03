using Iserv.Niis.Domain.Entities.ProtectionDoc;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Commands;

namespace Iserv.Niis.WorkflowBusinessLogic.ProtectionDocuments
{
    public class UpdateProtectionDocCommand : BaseCommand
    {
        public void Execute (ProtectionDoc protectionDoc)
        {
            var requestRepository = Uow.GetRepository<ProtectionDoc>();

            requestRepository.Update(protectionDoc);

            Uow.SaveChanges();
        }
    }
}
