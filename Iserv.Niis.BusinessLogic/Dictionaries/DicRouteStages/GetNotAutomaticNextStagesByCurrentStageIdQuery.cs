using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Iserv.Niis.Domain.Entities.Dictionaries;
using Iserv.Niis.Domain.Entities.Other;
using Microsoft.EntityFrameworkCore;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Queries;

namespace Iserv.Niis.BusinessLogic.Dictionaries.DicRouteStages
{
    public class GetNotAutomaticNextStagesByCurrentStageIdQuery : BaseQuery
    {
        public async Task<List<DicRouteStage>> ExecuteAsync(int stageId)
        {
            var repo = Uow.GetRepository<RouteStageOrder>();
            var nextStageids = await repo.AsQueryable()
                .Where(o => o.CurrentStageId == stageId && o.IsAutomatic == false)
                .Select(o => o.NextStageId)
                .ToListAsync();

            var stagesRepo = Uow.GetRepository<DicRouteStage>();
            var nextStages = await stagesRepo.AsQueryable()
                .Where(s => nextStageids.Contains(s.Id))
                .OrderBy(s => s.Code)
                .ToListAsync();

            return nextStages;
        }
    }
}