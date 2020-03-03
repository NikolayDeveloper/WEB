using Iserv.Niis.Domain.Entities.Workflow;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iserv.Niis.WorkflowBusinessLogic.WorkflowTaskQueues
{
    class GetLastWorkflowTaskQueuesByRequestIdAndCodeQuery : BaseQuery
    {
        public WorkflowTaskQueue Execute(int requestId, string conditionStageCode = null)
        {
            var workflowTaskQueueRepository = Uow.GetRepository<WorkflowTaskQueue>();
            var workflowTaskQueueQuery = workflowTaskQueueRepository
                .AsQueryable()
                .Where(r=>r.RequestId == requestId);

            if(string.IsNullOrEmpty(conditionStageCode) == false)
            {
                workflowTaskQueueQuery = workflowTaskQueueQuery.Where(r => r.ConditionStage != null && r.ConditionStage.Code == conditionStageCode);
            }

            return workflowTaskQueueQuery.LastOrDefault();
        }
    }
}