using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Iserv.Niis.Domain.Entities.Request;
using Microsoft.EntityFrameworkCore;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Queries;

namespace Iserv.Niis.BusinessLogic.Bibliographics.ConventionInfos.Request
{
    public class GetConventionInfosByRequestIdQuery: BaseQuery
    {
        public async Task<List<RequestConventionInfo>> ExecuteAsync(int requestId)
        {
            var requestConventionInfoRepository = Uow.GetRepository<RequestConventionInfo>();

            return await requestConventionInfoRepository.AsQueryable()
                .Where(pdci => pdci.RequestId == requestId)
                .ToListAsync();
        }
    }
}
