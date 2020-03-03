using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Iserv.Niis.Domain.Entities.Other;
using Microsoft.EntityFrameworkCore;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Queries;

namespace Iserv.Niis.WorkflowBusinessLogic.Dictionaries.DicRouteStage
{
    public class GetPreviousDicRouteStageOrdersByCodeQuery: BaseQuery
    {
        public List<RouteStageOrder> Execute(string currentStageCode)
        {
            var routeStageOrderRepository = Uow.GetRepository<RouteStageOrder>();

            var stages = routeStageOrderRepository.AsQueryable()
                .Where(r => r.NextStage != null && r.NextStage.Code == currentStageCode)
                .Include(r => r.NextStage)
                .Include(r => r.CurrentStage)
                .ToList();

            return stages;
        }
    }
}
