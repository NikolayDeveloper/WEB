using System.Collections.Generic;
using System.Linq;
using Iserv.Niis.Domain.Entities.Dictionaries;
using Iserv.Niis.Domain.Entities.Other;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Queries;

namespace Iserv.Niis.BusinessLogic.Dictionaries.DicRouteStages
{
    public class GetAllNextStagesByCurrentStageIdQuery : BaseQuery
    {
        public List<DicRouteStage> Execute(int stageId)
        {
            var repo = Uow.GetRepository<RouteStageOrder>();
            var nextStageIds = repo.AsQueryable()
                .Where(o => o.CurrentStageId == stageId)
                .Select(o => o.NextStageId)
                .ToList();

            var stagesRepo = Uow.GetRepository<DicRouteStage>();
            var nextStages = stagesRepo.AsQueryable()
                .Where(s => nextStageIds.Contains(s.Id))
                .ToList();

            return nextStages;
        }
    }
}
