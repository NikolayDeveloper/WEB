using System.Collections.Generic;
using System.Threading.Tasks;
using Iserv.Niis.Domain.Entities.Other;
using Microsoft.EntityFrameworkCore;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Queries;

namespace Iserv.Niis.BusinessLogic.Dictionaries.RouteStageOrders
{
    public class GetRouteStageOrderAllQuery : BaseQuery
    {
        public async Task<List<RouteStageOrder>> Execute()
        {
            var repo = Uow.GetRepository<RouteStageOrder>();

            return await repo
                .AsQueryable()
                .ToListAsync();
        }
    }
}