using Iserv.Niis.Domain.Entities.Workflow;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iserv.Niis.WorkflowBusinessLogic.WorkflowTaskQueues
{
    public class CreateWorkflowTaskQueueCommand : BaseCommand
    {
        public void Execute(WorkflowTaskQueue workflowTaskQueue)
        {
            var workflowTaskQueueRepository = Uow.GetRepository<WorkflowTaskQueue>();

            workflowTaskQueueRepository.Create(workflowTaskQueue);

            Uow.SaveChanges();
        }
    }
}
