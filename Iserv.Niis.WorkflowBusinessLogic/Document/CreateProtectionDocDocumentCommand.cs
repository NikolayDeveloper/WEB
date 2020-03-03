using Iserv.Niis.Domain.Entities.ProtectionDoc;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Commands;

namespace Iserv.Niis.WorkflowBusinessLogic.Document
{
    public class CreateProtectionDocDocumentCommand: BaseCommand
    {
        public void Execute(ProtectionDocDocument protectionDocDocument)
        {
            var repo = Uow.GetRepository<ProtectionDocDocument>();

            repo.Create(protectionDocDocument);
            Uow.SaveChanges();
        }
    }
}
