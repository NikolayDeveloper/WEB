using System.Threading.Tasks;
using Iserv.Niis.DataBridge.Implementations;
using Iserv.Niis.Domain.Entities.Request;
using Microsoft.EntityFrameworkCore;
//using NetCoreCQRS.Queries;

namespace Iserv.Niis.BusinessLogic.Requests
{
    /// <summary>
    /// Запрос для получения заявки по его идентификатору.
    /// </summary>
    public class GetBaseRequestByIdQuery : BaseQuery
    {
        /// <summary>
        /// Выполнение запроса.
        /// </summary>
        /// <param name="requestId">Идентификатор заявки.</param>
        /// <returns>Заявка.</returns>
        public async Task<Request> ExecuteAsync(int requestId)
        {
            var repository = Uow.GetRepository<Request>();

            var result = repository.AsQueryable()
                .FirstOrDefaultAsync(r => r.Id == requestId);

            return await result;
        }
    }
}