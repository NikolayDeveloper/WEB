using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Iserv.Niis.Domain.Entities.ManyToManyMappingEntities;
using Iserv.Niis.Domain.Entities.Request;
using Microsoft.EntityFrameworkCore;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Queries;

namespace Iserv.Niis.BusinessLogic.Requests
{
    public class GetChildRequestsByParentRequestIdQuery: BaseQuery
    {
        public async Task<List<Request>> Execute(int parentId)
        {
            var repo = Uow.GetRepository<RequestRequestRelation>();
            var childIds = repo.AsQueryable()
                .Where(rr => rr.ParentId == parentId).Select(rr => rr.ChildId);

            var requestRepo = Uow.GetRepository<Request>();

            var result = requestRepo.AsQueryable()
                .Include(r => r.CurrentWorkflow).ThenInclude(cw => cw.CurrentUser)
                .Include(r => r.CurrentWorkflow).ThenInclude(cw => cw.CurrentStage)
                .Include(r => r.Workflows).ThenInclude(w => w.CurrentUser)
                .Include(r => r.Workflows).ThenInclude(w => w.CurrentStage)
                .Include(r => r.Documents).ThenInclude(rd => rd.Document).ThenInclude(d => d.Type)
                .Include(r => r.ProtectionDocType)
                .Where(r => childIds.Contains(r.Id));

            return await result.ToListAsync();
        }
    }
}
