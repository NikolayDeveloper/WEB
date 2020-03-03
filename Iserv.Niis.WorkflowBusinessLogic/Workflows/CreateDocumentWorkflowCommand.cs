using Iserv.Niis.Domain.Entities.Document;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Commands;

namespace Iserv.Niis.WorkflowBusinessLogic.Workflows
{
    public class CreateDocumentWorkflowCommand : BaseCommand
    {
        public int Execute(DocumentWorkflow workflow)
        {
            var requestWorkflowRepository = Uow.GetRepository<DocumentWorkflow>();

            requestWorkflowRepository.Create(workflow);

            Uow.SaveChanges();

            return workflow.Id;
        }
    }
}
