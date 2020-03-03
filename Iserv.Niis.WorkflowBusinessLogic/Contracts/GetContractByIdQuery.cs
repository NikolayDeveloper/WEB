using Iserv.Niis.Domain.Entities.Contract;
using Microsoft.EntityFrameworkCore;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Queries;
using System.Linq;

namespace Iserv.Niis.WorkflowBusinessLogic.Contracts
{
    public class GetContractByIdQuery : BaseQuery
    {
        public Contract Execute(int requestId)
        {
            var repository = Uow.GetRepository<Contract>();

            return repository.AsQueryable()
                .Include(r => r.CurrentWorkflow).ThenInclude(r => r.CurrentStage)
                .Include(r => r.CurrentWorkflow).ThenInclude(r => r.FromStage)
                .Include(r => r.CurrentWorkflow).ThenInclude(r => r.Route)
                .Include(r => r.PaymentInvoices)
                .Include(r => r.Documents).ThenInclude(d => d.Document).ThenInclude(d => d.Type)
                .FirstOrDefault(r => r.Id == requestId);
        }
    }
}
