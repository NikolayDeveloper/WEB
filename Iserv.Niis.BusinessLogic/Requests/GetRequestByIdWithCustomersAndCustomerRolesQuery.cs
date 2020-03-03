using Iserv.Niis.Domain.Entities.Request;
using Microsoft.EntityFrameworkCore;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Queries;
using NetCoreDataAccess.Repository;
using System.Threading.Tasks;

namespace Iserv.Niis.BusinessLogic.Requests
{
    /// <summary>
    /// Запрос возвращающий заявку по ее идентификатору
    /// и привязанных к ней контрагентов с их ролями.
    /// </summary>
    public class GetRequestByIdWithCustomersAndCustomerRolesQuery : BaseQuery
    {
        /// <summary>
        /// Выполнение запроса.
        /// </summary>
        /// <param name="requestId">Идентификатор заявки.</param>
        /// <returns>Заявка.</returns>
        public async Task<Request> ExecuteAsync(int requestId)
        {
            IRepository<Request> repository = Uow.GetRepository<Request>();

            return await repository.AsQueryable()
                .Include(request => request.RequestCustomers)
                .ThenInclude(customer => customer.CustomerRole)
                .FirstOrDefaultAsync(request => request.Id == requestId);
        }
    }
}
