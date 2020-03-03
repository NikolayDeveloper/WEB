using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Iserv.Niis.Domain.Entities.Contract;
using Microsoft.EntityFrameworkCore;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Queries;

namespace Iserv.Niis.BusinessLogic.Workflows.Contracts
{
    public class GetContractWorkflowsByOwnerIdQuery : BaseQuery
    {
        public async ValueTask<List<ContractWorkflow>> ExecuteAsync(int ownerId)
        {
            var contractWorkflowRepository = Uow.GetRepository<ContractWorkflow>();
            var contractWorkflows = await contractWorkflowRepository
                .AsQueryable()
                .Include(r => r.FromStage)
                .Include(r => r.CurrentStage)
                .Include(r => r.FromUser)
                .Include(r => r.CurrentUser)
                .Include(r => r.Route)
                .Where(rc => rc.OwnerId == ownerId)
                .OrderBy(rc => rc.DateCreate)
                .ToListAsync();

            return contractWorkflows;
        }
    }
}