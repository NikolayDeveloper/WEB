using System.Linq;
using Iserv.Niis.Domain.Entities.Security;
using Iserv.Niis.WorkflowBusinessLogic.Workflows;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Handlers;

namespace Iserv.Niis.WorkflowBusinessLogic.Requests
{
    public class GetRequestAutoRouteStageExecutorHandler: BaseHandler
    {
        public ApplicationUser Execute(int stageId)
        {
            //var request = Executor.GetQuery<GetRequestByIdQuery>().Process(q => q.Execute(requestid));
            var executors = Executor.GetQuery<GetRequestAutoRouteStageExecutorsQuery>().Process(q => q.Execute());
            var executorPosition = executors.SingleOrDefault(e => e.StageId == stageId)?.Position;
            if (executorPosition == null) return null;

            var allStageExecutors = Executor.GetQuery<GetWorkflowStageUsersQuery>().Process(q => q.Execute(stageId));

            return allStageExecutors.Where(d => d.PositionId == executorPosition.Id).FirstOrDefault(u => u.IsArchive != true && u.IsDeleted == false);
        }
    }
}
