using Iserv.Niis.Domain.Entities.ProtectionDoc;
using Microsoft.EntityFrameworkCore;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Commands;
using System.Linq;
using System.Threading.Tasks;
using Iserv.Niis.DataBridge.Implementations;

namespace Iserv.Niis.Model.BusinessLogic
{
    public class GetProtectionDocWorkflowFromParalleByOwnerIdCommand : BaseCommand
    {
        public ProtectionDocWorkflow Execute(int ownerId, long userId)
        {
            var requestParallelWorkflowRepository = Uow.GetRepository<ProtectionDocParallelWorkflow>();
            var parallelWorkflows = requestParallelWorkflowRepository.AsQueryable();

            var requestProtectionDocWorkflow = Uow.GetRepository<ProtectionDocWorkflow>();

            var appParallelWorkflowIds = parallelWorkflows
                .Where(x => x.OwnerId == ownerId && !x.IsFinished)
                .Select(x => x.ProtectionDocWorkflowId).ToArray();

            ProtectionDocWorkflow workflow = requestProtectionDocWorkflow
                .AsQueryable()
                .Include(r => r.CurrentStage)
                .Include(cw => cw.FromStage)
                .Include(cw => cw.CurrentUser).ThenInclude(u => u.Department).ThenInclude(div => div.Division)
                .Where(x => appParallelWorkflowIds.Contains(x.Id)).FirstOrDefault(x => x.CurrentUserId.HasValue && x.CurrentUserId.Value == userId);

            return workflow;
        }
    }
}
