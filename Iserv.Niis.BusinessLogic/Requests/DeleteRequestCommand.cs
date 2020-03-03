using Iserv.Niis.Domain.Entities.Request;
using Iserv.Niis.Exceptions;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Commands;

namespace Iserv.Niis.BusinessLogic.Requests
{
    public class DeleteRequestCommand : BaseCommand
    {
        public void Execute(int requestId)
        {
            var requestRepository = Uow.GetRepository<Request>();
            var request = requestRepository.GetById(requestId);

            if (request == null)
            {
                throw new DataNotFoundException(nameof(Request), DataNotFoundException.OperationType.Delete, requestId);
            }

            requestRepository.Delete(request);
            Uow.SaveChanges();
        }
    }
}
