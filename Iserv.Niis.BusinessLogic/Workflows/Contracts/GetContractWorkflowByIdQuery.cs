using Iserv.Niis.Domain.Entities.Contract;
using Microsoft.EntityFrameworkCore;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Queries;
using System.Threading.Tasks;

namespace Iserv.Niis.BusinessLogic.Workflows.ProtectionDoc
{
    public class GetContractWorkflowByIdQuery : BaseQuery
    {
        public async Task<ContractWorkflow> ExecuteAsync(int id)
        {
            var contractWorkflowRepository = Uow.GetRepository<ContractWorkflow>();
            var protectionDocWorkflows = contractWorkflowRepository
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
