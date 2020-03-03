using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Iserv.Niis.Domain.Entities.Dictionaries;
using Microsoft.EntityFrameworkCore;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Queries;

namespace Iserv.Niis.BusinessLogic.Dictionaries.DicRouteStages
{
    public class GetAllRouteStageByRouteQuery : BaseQuery
    {
        public async Task<List<DicRouteStage>> ExecuteAsync(int routeId)
        {
            var stagesRepo = Uow.GetRepository<DicRouteStage>();
            var nextStages = await stagesRepo.AsQueryable()
                .Where(s => s.RouteId == routeId && s.IsDeleted == false)
                .ToListAsync();

            return nextStages;
        }
    }
}