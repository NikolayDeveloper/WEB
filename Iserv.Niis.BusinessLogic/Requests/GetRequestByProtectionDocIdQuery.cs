using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Iserv.Niis.Domain.Entities.ProtectionDoc;
using Iserv.Niis.Domain.Entities.Request;
using Microsoft.EntityFrameworkCore;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Queries;

namespace Iserv.Niis.BusinessLogic.Requests
{
    public class GetRequestByProtectionDocIdQuery: BaseQuery
    {
        public async Task<Request> ExecuteAsync(int protectionDocId)
        {
            var repo = Uow.GetRepository<ProtectionDoc>();
            var request = await repo.AsQueryable()
                .Include(pd => pd.Request).ThenInclude(r => r.CurrentWorkflow).ThenInclude(cw => cw.CurrentStage)
                .Include(pd => pd.Request).ThenInclude(r => r.CurrentWorkflow).ThenInclude(cw => cw.CurrentUser)
                .Include(pd => pd.Request).ThenInclude(r => r.Workflows).ThenInclude(cw => cw.CurrentStage)
                .Include(pd => pd.Request).ThenInclude(r => r.Workflows).ThenInclude(cw => cw.CurrentUser)
                .Include(pd => pd.Request).ThenInclude(r => r.ProtectionDocType)
                .Where(r => r.Id == protectionDocId)
                .Select(pd => pd.Request)
                .FirstOrDefaultAsync();
            
            return request;
        }
    }
}
