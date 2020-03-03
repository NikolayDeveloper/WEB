using Iserv.Niis.Domain.Entities.Request;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Commands;

namespace Iserv.Niis.BusinessLogic.Requests
{
    public class UpdateRequestAsReadCommand : BaseCommand
    {
        public void Execute(int requestId)
        {
            var requestRepository = Uow.GetRepository<Request>();
            var request = requestRepository.GetById(requestId);

            request.IsRead = true;

            requestRepository.Update(request);
            Uow.SaveChanges();
        }
    }
}
