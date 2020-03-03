using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Iserv.Niis.Domain.Entities.ManyToManyMappingEntities;
using Iserv.Niis.Domain.Entities.Request;
using Microsoft.EntityFrameworkCore;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Queries;

namespace Iserv.Niis.BusinessLogic.Requests
{
    public class GetParentRequestsByChildRequestIdQuery: BaseQuery
    {
        public async Task<List<Request>> Execute(int childId)
        {
            var repo = Uow.GetRepository<RequestRequestRelation>();
            var childIds = repo.AsQueryable()
                .Where(rr => rr.ChildId == childId).Select(rr => rr.ParentId);

            var requestRepo = Uow.GetRepository<Request>();

            var result = requestRepo.AsQueryable()
                .Include(r => r.CurrentWorkflow).ThenInclude(cw => cw.CurrentUser)
                .Include(r => r.CurrentWorkflow).ThenInclude(cw => cw.CurrentStage)
                .Include(r => r.Workflows).ThenInclude(w => w.CurrentUser)
                .Include(r => r.Workflows).ThenInclude(w => w.CurrentStage)
                .Include(r => r.PaymentInvoices).ThenInclude(pi => pi.Status)
                .Include(r => r.PaymentInvoices).ThenInclude(pi => pi.Tariff)
                .Include(r => r.ProtectionDocType)
                .Where(r => childIds.Contains(r.Id));

            return await result.ToListAsync();
        }
    }
}
