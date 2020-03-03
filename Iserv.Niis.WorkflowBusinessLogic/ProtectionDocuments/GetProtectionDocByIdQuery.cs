using Iserv.Niis.Domain.Entities.ProtectionDoc;
using Microsoft.EntityFrameworkCore;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Queries;
using System.Linq;

namespace Iserv.Niis.WorkflowBusinessLogic.ProtectionDocuments
{
    public class GetProtectionDocByIdQuery : BaseQuery
    {
        public ProtectionDoc Execute(int requestId)
        {
            var repository = Uow.GetRepository<ProtectionDoc>();

            return repository.AsQueryable()
                .Include(r => r.CurrentWorkflow).ThenInclude(r => r.CurrentStage)
                .Include(r => r.CurrentWorkflow).ThenInclude(r => r.FromStage)
                .Include(r => r.CurrentWorkflow).ThenInclude(r => r.Route)
                .Include(pd => pd.ProtectionDocCustomers).ThenInclude(pdc => pdc.CustomerRole)
                .Include(r => r.Documents).ThenInclude(pd => pd.Document).ThenInclude(d => d.Type)
                .Include(pd => pd.Bulletins).ThenInclude(pdb => pdb.Bulletin)
                .Include(r => r.Type)
                .Include(r => r.PaymentInvoices)
                .FirstOrDefault(r => r.Id == requestId);
        }
    }
}
