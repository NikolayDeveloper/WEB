using Iserv.Niis.Domain.Entities.Request;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Commands;

namespace Iserv.Niis.WorkflowBusinessLogic.Requests
{
    /// <summary>
    /// Класс, который представляет запрос на обновление данных о заявке.
    /// </summary>
    public class UpdateRequestCommand : BaseCommand
    {
        /// <summary>
        /// Выполнение запроса.
        /// </summary>
        /// <param name="request">Заявка.</param>
        public void Execute (Request request)
        {
            var requestRepository = Uow.GetRepository<Request>();

            requestRepository.Update(request);

            Uow.SaveChanges();
        }
    }
}
