using System.Linq;
using System.Threading.Tasks;
using Iserv.Niis.Domain.Entities.Request;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Queries;

namespace Iserv.Niis.BusinessLogic.Requests
{
    public class GetICGSRequestsByRequestIdsQuery : BaseQuery
    {
        public async Task<IQueryable<ICGSRequest>> ExecuteAsync(params int[] requestIds)
        {
            var repository = Uow.GetRepository<ICGSRequest>();
            var icgsRequests = repository.AsQueryable().Where(ir => requestIds.Contains(ir.RequestId));

            var result = await Task.FromResult(icgsRequests);

            return result;
        }
    }
}