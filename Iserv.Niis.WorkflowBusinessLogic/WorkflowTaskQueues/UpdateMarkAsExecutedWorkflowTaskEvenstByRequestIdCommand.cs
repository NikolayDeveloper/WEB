using Iserv.Niis.Domain.Entities.Workflow;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Commands;
using System.Linq;

namespace Iserv.Niis.WorkflowBusinessLogic.WorkflowTaskQueues
{
    /// <summary>
    /// Класс, который представляет запрос, который помечает все запланированные задачи связанные с заявкой, как выполненные по идентификатору заявки.
    /// </summary>
    public class UpdateMarkAsExecutedWorkflowTaskEvenstByRequestIdCommand : BaseCommand
    {
        /// <summary>
        /// Выполнение запроса.
        /// </summary>
        /// <param name="requestId">Идентификатор заявки.</param>
        public void Execute(int requestId)
        {
            var workflowTaskQueueRepository = Uow.GetRepository<WorkflowTaskQueue>();

            var notExecutedTaskList = workflowTaskQueueRepository.AsQueryable().Where(r=>
                                                                r.RequestId == requestId 
                                                                && (r.IsExecuted == null || r.IsExecuted == false));

            foreach (var notExecutedTask in notExecutedTaskList)
            {
                notExecutedTask.IsExecuted = true;
            }

            Uow.SaveChanges();
        }
    }
}
