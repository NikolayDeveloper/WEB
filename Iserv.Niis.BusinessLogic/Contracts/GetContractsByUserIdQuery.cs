using System.Linq;
using Iserv.Niis.Domain.Entities.Contract;
using Microsoft.EntityFrameworkCore;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Queries;

namespace Iserv.Niis.BusinessLogic.Contracts
{
    public class GetContractsByUserIdQuery: BaseQuery
    {
        public IQueryable<Contract> Execute(int userId)
        {
            var repository = Uow.GetRepository<Contract>();

            return repository.AsQueryable()
                .Include(r => r.CurrentWorkflow).ThenInclude(cw => cw.CurrentStage)
                .Include(r => r.CurrentWorkflow).ThenInclude(cw => cw.CurrentUser)
                .Include(c => c.Type)
                .Include(r => r.ProtectionDocType)
                .Where(r => r.CurrentWorkflowId != null && r.CurrentWorkflow.CurrentUserId == userId);
        }
    }
}
