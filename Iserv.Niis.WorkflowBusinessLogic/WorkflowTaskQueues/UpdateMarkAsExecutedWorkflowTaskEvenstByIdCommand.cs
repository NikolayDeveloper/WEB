using Iserv.Niis.Domain.Entities.Workflow;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Commands;
using System.Linq;

namespace Iserv.Niis.WorkflowBusinessLogic.WorkflowTaskQueues
{
    public class UpdateMarkAsExecutedWorkflowTaskEvenstByIdCommand : BaseCommand
    {
        public void Execute(int id)
        {
            var workflowTaskQueueRepository = Uow.GetRepository<WorkflowTaskQueue>();

            var notExecutedTask = workflowTaskQueueRepository.AsQueryable().FirstOrDefault(r =>r.Id == id
                                                                && (r.IsExecuted == null || r.IsExecuted == false));

            if(notExecutedTask != null)
            {
                notExecutedTask.IsExecuted = true;
            }

            Uow.SaveChanges();
        }
    }
}
