using Iserv.Niis.Domain.Entities.Document;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Commands;

namespace Iserv.Niis.WorkflowBusinessLogic.Workflows
{
    public class UpdateDocumentWokFlowCommand : BaseCommand
    {
        public void Execute(DocumentWorkflow documentWorkflow)
        {
            var repo = Uow.GetRepository<DocumentWorkflow>();
            repo.Update(documentWorkflow);

            Uow.SaveChanges();
        }
    }
}
