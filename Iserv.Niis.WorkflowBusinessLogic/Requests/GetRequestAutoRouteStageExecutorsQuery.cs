using System.Linq;
using Iserv.Niis.Domain.Entities.AutoRouteStages;
using Microsoft.EntityFrameworkCore;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Queries;

namespace Iserv.Niis.WorkflowBusinessLogic.Requests
{
    public class GetRequestAutoRouteStageExecutorsQuery: BaseQuery
    {
        public IQueryable<RequestAutoRouteStageExecutor> Execute()
        {
            var repo = Uow.GetRepository<RequestAutoRouteStageExecutor>();

            return repo.AsQueryable()
                .Include(r => r.Position)
                .Include(r => r.Stage);
        }
    }
}
