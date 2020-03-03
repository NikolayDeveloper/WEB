using Iserv.Niis.Domain.Entities.ProtectionDoc;
using Microsoft.EntityFrameworkCore;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Queries;
using System.Threading.Tasks;

namespace Iserv.Niis.BusinessLogic.Workflows.ProtectionDoc
{
    public class GetProtectionDocWorkflowByIdQuery : BaseQuery
    {
        public async Task<ProtectionDocWorkflow> ExecuteAsync(int id)
        {
            var protectionDocWorkflowRepository = Uow.GetRepository<ProtectionDocWorkflow>();
            var protectionDocWorkflows = protectionDocWorkflowRepository
                .AsQueryable()
                .Include(r => r.FromStage)
                .Include(r => r.CurrentStage)
                .Include(r => r.FromUser)
                .Include(r => r.CurrentUser)
                .Include(r => r.Route)
                .Include(r => r.Owner)
                .FirstOrDefaultAsync(r => r.Id == id);

            return await protectionDocWorkflows;
        }
    }
}
