using Iserv.Niis.Domain.Entities.Other;
using Microsoft.EntityFrameworkCore;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Queries;
using System.Collections.Generic;
using System.Linq;

namespace Iserv.Niis.WorkflowBusinessLogic.Dictionaries.DicRouteStage
{
    public class GetNextDicRouteStageOrdersByCodeQuery : BaseQuery
    {
        public List<RouteStageOrder> Execute(string currentStageCode)
        {
            var routeStageOrderRepository = Uow.GetRepository<RouteStageOrder>();

            var stages = routeStageOrderRepository.AsQueryable()
                                                    .Where(r => r.CurrentStage != null && r.CurrentStage.Code == currentStageCode)
                                                    .Include(r=>r.NextStage)
                                                    .Include(r=>r.CurrentStage)
                                                    .ToList();

            return stages;
        }
    }
}
