using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Iserv.Niis.Domain.Entities.Workflow;
using Microsoft.EntityFrameworkCore;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Queries;

namespace Iserv.Niis.BusinessLogic.Workflows
{
    public class GetWorkflowTaskQueuesByRequestIdQuery : BaseQuery
    {
        public async Task<List<WorkflowTaskQueue>> Execute(int requestId)
        {
            var result = Uow.GetRepository<WorkflowTaskQueue>().AsQueryable()
                .Include(a => a.ConditionStage)
                .Include(a => a.ResultStage)
                .Where(t => t.RequestId.HasValue && t.RequestId == requestId)
                .ToListAsync();

            return await result;
        }
    }
}