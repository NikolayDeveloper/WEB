using Iserv.Niis.Domain.Entities.Request;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Commands;

namespace Iserv.Niis.WorkflowBusinessLogic.Workflows
{
    public class UpdateRequestWorkflowCommand: BaseCommand
    {
        public void Execute(RequestWorkflow requestWorkflow)
        {
            var repo = Uow.GetRepository<RequestWorkflow>();
            repo.Update(requestWorkflow);

            Uow.SaveChanges();
        }
    }
}
