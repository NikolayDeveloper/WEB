using System.Collections.Generic;
using System.Threading.Tasks;
using Iserv.Niis.Domain.Entities.Dictionaries.DicMain;
using Microsoft.EntityFrameworkCore;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Queries;
using Iserv.Niis.DataBridge.Implementations;

namespace Iserv.Niis.BusinessLogic.Dictionaries.DicRoutes
{
    public class GetDicRoutesQuery : BaseQuery
    {
        public async Task<List<DicRoute>> ExecuteAsync()
        {
            var routeRepo = Uow.GetRepository<DicRoute>();
            return await routeRepo
                .AsQueryable()
                .Include(r => r.RouteStages)
                .ToListAsync();
        }
    }
}