using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Iserv.Niis.Domain.Entities.Dictionaries;
using Iserv.Niis.Domain.Entities.Other;
using Iserv.Niis.Domain.Entities.ProtectionDoc;
using Microsoft.EntityFrameworkCore;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Queries;

namespace Iserv.Niis.BusinessLogic.Dictionaries.DicRouteStages
{
    public class GetNextStagesByCurrentUserIdQuery: BaseQuery
    {
        public async Task<List<DicRouteStage>> ExecuteAsync(int ownerId, int userId)
        {
            var requestParallelWorkflowRepository = Uow.GetRepository<ProtectionDocParallelWorkflow>();
            var parallelWorkflows = requestParallelWorkflowRepository.AsQueryable();

            var requestProtectionDocWorkflow = Uow.GetRepository<ProtectionDocWorkflow>();

            var appParallelWorkflowIds = parallelWorkflows
                .Where(x => x.OwnerId == ownerId && !x.IsFinished)
                .Select(x => x.ProtectionDocWorkflowId).ToArray();

            var stageId = requestProtectionDocWorkflow
                .AsQueryable()
                .Include(r => r.CurrentStage)
                .Include(cw => cw.FromStage)
                .Include(cw => cw.CurrentUser).ThenInclude(u => u.Department).ThenInclude(div => div.Division)
                .Where(x => appParallelWorkflowIds.Contains(x.Id) && x.CurrentUserId != null && x.CurrentUserId == userId).Select(x => x.CurrentStageId).FirstOrDefault();

            stageId = stageId.HasValue ? stageId.Value : default(int);

            var repo = Uow.GetRepository<RouteStageOrder>();

            var nextStageids = await repo.AsQueryable()
                .Where(o => o.CurrentStageId == stageId && o.IsAutomatic == false)
                .Select(o => o.NextStageId)
                .ToListAsync();

            var stagesRepo = Uow.GetRepository<DicRouteStage>();
            var nextStages = await stagesRepo.AsQueryable()
                .Where(s => nextStageids.Contains(s.Id))
                .ToListAsync();

            return nextStages;
        }
    }
}
