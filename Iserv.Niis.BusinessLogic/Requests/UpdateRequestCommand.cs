using Iserv.Niis.Domain.Entities.Request;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Commands;
using System.Threading.Tasks;

namespace Iserv.Niis.BusinessLogic.Requests
{
    public class UpdateRequestCommand : BaseCommand
    {
        public async Task<int> ExecuteAsync(Request request)
        {
            var requestRepository = Uow.GetRepository<Request>();
            requestRepository.Update(request);
            await Uow.SaveChangesAsync();
            return request.Id;
        }
    }
}
