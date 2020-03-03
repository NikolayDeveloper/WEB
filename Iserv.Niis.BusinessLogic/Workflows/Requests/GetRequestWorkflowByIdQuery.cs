using Iserv.Niis.Domain.Entities.Contract;
using Iserv.Niis.Domain.Entities.Request;
using Microsoft.EntityFrameworkCore;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Queries;
using System.Threading.Tasks;

namespace Iserv.Niis.BusinessLogic.Workflows.ProtectionDoc
{
    public class GetRequestWorkflowByIdQuery : BaseQuery
    {
        public async Task<RequestWorkflow> ExecuteAsync(int id)
        {
            var requestWorkflowRepository = Uow.GetRepository<RequestWorkflow>();
            var protectionDocWorkflows = requestWorkflowRepository
                .AsQueryable()
                .Include(r => r.FromStage)
                .Include(r => r.CurrentStage)
                .Include(r => r.FromUser)
                .Include(r => r.CurrentUser)
                .Include(r => r.Route)
                .Include(r => r.Owner)
                .FirstOrDefaultAsync(w => w.Id == id);

            return await protectionDocWorkflows;
        }
    }
}
