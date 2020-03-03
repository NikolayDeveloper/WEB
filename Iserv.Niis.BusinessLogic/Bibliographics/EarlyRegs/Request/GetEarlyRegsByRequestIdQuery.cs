using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Iserv.Niis.Domain.Entities.Request;
using Microsoft.EntityFrameworkCore;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Queries;

namespace Iserv.Niis.BusinessLogic.Bibliographics.EarlyRegs.Request
{
    public class GetEarlyRegsByRequestIdQuery: BaseQuery
    {
        public async Task<List<RequestEarlyReg>> ExecuteAsync(int requestId)
        {
            var requestEarlyRegRepository = Uow.GetRepository<RequestEarlyReg>();

            return await requestEarlyRegRepository.AsQueryable()
                .Where(er => er.RequestId == requestId)
                .ToListAsync();
        }
    }
}
