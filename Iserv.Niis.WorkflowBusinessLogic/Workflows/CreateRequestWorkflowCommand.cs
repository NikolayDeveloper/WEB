using Iserv.Niis.Domain.Entities.Request;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Commands;

namespace Iserv.Niis.WorkflowBusinessLogic.Workflows
{
    public class CreateRequestWorkflowCommand : BaseCommand
    {
        public void Execute(RequestWorkflow requestWorkflow)
        {
            var requestWorkflowRepository = Uow.GetRepository<RequestWorkflow>();

            requestWorkflowRepository.Create(requestWorkflow);

            Uow.SaveChanges();
        }
    }
}
