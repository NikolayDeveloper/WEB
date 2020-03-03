using Iserv.Niis.Domain.Entities.Request;
using Microsoft.EntityFrameworkCore;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Queries;
using System.Threading.Tasks;

namespace Iserv.Niis.BusinessLogic.Requests
{
    public class GetRequsetImageByIdQuery : BaseQuery
    {
        public async Task<Request> ExecuteAsync(int requsetId)
        {
            var repository = Uow.GetRepository<Request>();

            return await repository.AsQueryable().FirstOrDefaultAsync(r => r.Id == requsetId);
        }
    }
}
