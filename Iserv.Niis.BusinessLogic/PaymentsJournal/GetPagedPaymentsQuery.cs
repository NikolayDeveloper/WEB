using Iserv.Niis.BusinessLogic.PaymentsJournal.Dto;
using Iserv.Niis.Infrastructure.Pagination;
using Microsoft.AspNetCore.Http;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; ;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Queries;

namespace Iserv.Niis.BusinessLogic.PaymentsJournal
{
    /// <summary>
    /// Запрос для поиска, сортировки и пагинации платежей по определенным фильтрам.
    /// </summary>
    public class GetPagedPaymentsQuery : BaseQuery
    {
        private readonly IExecutor _executor;
        public GetPagedPaymentsQuery(IExecutor executor)
        {
            _executor = executor;
        }

        /// <summary>
        /// Выполнение запроса.
        /// </summary>
        /// <param name="searchParameters">Фильтры поиска.</param>
        /// <param name="httpRequest">Запрос.</param>
        /// <returns>Список с платежами.</returns>
        public IPagedList<PaymentDto> Execute(PaymentsSearchParametersDto searchParameters, HttpRequest httpRequest)
        {
            var payments = _executor
                .GetQuery<GetPaymentsQuery>()
                .Process(query => query.Execute(searchParameters, httpRequest));

            return payments.ToPagedList(httpRequest.GetPaginationParams());
        }
    }
}