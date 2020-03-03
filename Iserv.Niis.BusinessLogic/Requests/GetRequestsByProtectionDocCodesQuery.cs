using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Iserv.Niis.Domain.Entities.Request;
using Microsoft.EntityFrameworkCore;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Queries;

namespace Iserv.Niis.BusinessLogic.Requests
{
    public class GetRequestsByProtectionDocCodesQuery : BaseQuery
    {
        public async Task<List<Request>> ExecuteAsync(params string[] protectionDocCode)
        {
            var repository = Uow.GetRepository<Request>();

            var result = await repository.AsQueryable()
                .Include(r => r.CurrentWorkflow).ThenInclude(cw => cw.CurrentStage)
                .Where(r => protectionDocCode.Contains(r.ProtectionDocType.Code))
                .ToListAsync();

            return result;
        }
    }
}