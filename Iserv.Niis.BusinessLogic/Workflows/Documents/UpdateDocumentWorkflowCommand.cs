using Iserv.Niis.Domain.Entities.Document;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Commands;

namespace Iserv.Niis.BusinessLogic.Workflows.Documents
{
    public class UpdateDocumentWorkflowCommand : BaseCommand
    {
        public void Execute(DocumentWorkflow workflow)
        {
            var requestWorkflowRepository = Uow.GetRepository<DocumentWorkflow>();

            requestWorkflowRepository.Update(workflow);

            Uow.SaveChanges();
        }
    }
}
