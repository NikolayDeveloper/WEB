using System.Linq;
using Iserv.Niis.Common.Codes;
using Iserv.Niis.Domain.Entities.ProtectionDoc;
using Microsoft.EntityFrameworkCore;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Queries;

namespace Iserv.Niis.BusinessLogic.ProtectionDocs
{
    public class GetProtectionDocsByUserIdQuery: BaseQuery
    {
        public IQueryable<ProtectionDoc> Execute(int userId)
        {
            var repository = Uow.GetRepository<ProtectionDoc>();
            var parallelWorkflowRepository = Uow.GetRepository<ProtectionDocParallelWorkflow>();
            var parallelWorkflowsQuery = parallelWorkflowRepository.AsQueryable();

            return repository.AsQueryable()
                .Include(r => r.Workflows).ThenInclude(w => w.CurrentUser)
                .Include(r => r.CurrentWorkflow).ThenInclude(cw => cw.CurrentStage)
                .Include(r => r.CurrentWorkflow).ThenInclude(cw => cw.CurrentUser)
                .Include(r => r.Type)
                .Include(r => r.IpcProtectionDocs).ThenInclude(ir => ir.Ipc)
                .Where(r => r.CurrentWorkflowId.HasValue && ((r.CurrentWorkflow.CurrentUserId.HasValue && r.CurrentWorkflow.CurrentUserId == userId )||
                                                            (r.CurrentWorkflow.SecondaryCurrentUserId.HasValue && r.CurrentWorkflow.SecondaryCurrentUserId == userId)) ||
                            new[] {RouteStageCodes.OD05, RouteStageCodes.OD05_01}.Contains(r.CurrentWorkflow
                                .CurrentStage.Code) ||
                            (r.BulletinUserId.HasValue && r.BulletinUserId == userId) ||
                            (r.SupportUserId.HasValue && r.SupportUserId == userId) ||
                            (r.Workflows.Join(parallelWorkflowsQuery,
                                w => w.Id,
                                pw => pw.ProtectionDocWorkflowId,
                                (workf, pWorkf) => new { workf, pWorkf }
                                ).Any(x => !x.pWorkf.IsFinished)
                             )
                );
        }
    }
}
