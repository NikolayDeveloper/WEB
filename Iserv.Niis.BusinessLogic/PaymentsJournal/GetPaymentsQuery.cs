using System.Linq;
using Iserv.Niis.BusinessLogic.PaymentsJournal.Dto;
using Iserv.Niis.Domain.Entities.Payment;
using Iserv.Niis.Infrastructure.Extensions.Filter;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Queries;

namespace Iserv.Niis.BusinessLogic.PaymentsJournal
{
    /// <summary>
    /// Запрос для получения платежей.
    /// </summary>
    public class GetPaymentsQuery : BaseQuery
    {
        /// <summary>
        /// Выполнение запроса.
        /// </summary>
        /// <param name="searchParameters">Фильтры поиска.</param>
        /// <param name="httpRequest">Запрос.</param>
        /// <returns>Список с платежами.</returns>
        public IQueryable<PaymentDto> Execute(PaymentsSearchParametersDto searchParameters, HttpRequest httpRequest)
        {
            return GetQuery(searchParameters)
                .Select(PaymentDto.FromPaymentDto)
                .Sort(httpRequest.Query);
        }

        /// <summary>
        /// Возвращает отфильрованный запрос к платежам.
        /// </summary>
        /// <param name="searchParameters">Параметры фильтрации.</param>
        /// <returns>Отфильтрованный запрос к платежам.</returns>
        private IQueryable<Payment> GetQuery(PaymentsSearchParametersDto searchParameters)
        {
            var query = Uow.GetRepository<Payment>()
                .AsQueryable()
                .AsNoTracking()
                .Include(r => r.Customer)
                .Include(r => r.PaymentStatus)
                .Include(r => r.PaymentUses);

            return ApplySearch(query, searchParameters);
        }

        /// <summary>
        /// Выполняет фильтрацию запроса.
        /// </summary>
        /// <param name="query">Запрос к платежам.</param>
        /// <param name="searchParams">Параметры фильтрации.</param>
        /// <returns>Отфильтрованный запрос к платежам.</returns>
        private static IQueryable<Payment> ApplySearch(IQueryable<Payment> query, PaymentsSearchParametersDto searchParams)
        {
            if (searchParams.Id != null)
                query = query.Where(x => x.Id.ToString().Contains(searchParams.Id.ToString()));

            if (searchParams.DateCreateFrom != null)
                query = query.Where(x => x.DateCreate >= searchParams.DateCreateFrom.Value.Date);

            if (searchParams.DateCreateTo != null)
                query = query.Where(x => x.DateCreate < searchParams.DateCreateTo.Value.Date.AddDays(1));

            if (!string.IsNullOrWhiteSpace(searchParams.PayerName))
                query = query.Where(x => x.Customer.NameRu.Contains(searchParams.PayerName));

            if (!string.IsNullOrWhiteSpace(searchParams.PayerXin))
                query = query.Where(x => x.Customer.Xin.Contains(searchParams.PayerXin));

            if (!string.IsNullOrWhiteSpace(searchParams.PayerRnn))
                query = query.Where(x => x.Customer.Rnn.Contains(searchParams.PayerRnn));

            if (searchParams.PaymentDateFrom != null)
                query = query.Where(x => x.PaymentDate >= searchParams.PaymentDateFrom.Value.Date);

            if (searchParams.PaymentDateTo != null)
                query = query.Where(x => x.PaymentDate < searchParams.PaymentDateTo.Value.Date.AddDays(1));

            if (searchParams.Amount != null)
                query = query.Where(x => x.Amount == searchParams.Amount);

            if (searchParams.Remainder != null)
            {
                query = query.Where(x =>
                (x.Amount - (x.PaymentUses.Any() ? x.PaymentUses.Sum(pu => pu.Amount) : 0)) == searchParams.Remainder);
            }

            if (searchParams.Distributed != null)
                query = query.Where(x => (x.PaymentUses.Any() ? x.PaymentUses.Sum(pu => pu.Amount) : 0) == searchParams.Distributed);

            if (!string.IsNullOrWhiteSpace(searchParams.PaymentPurpose))
                query = query.Where(x => x.PurposeDescription.Contains(searchParams.PaymentPurpose));

            if (!string.IsNullOrWhiteSpace(searchParams.PaymentNumber))
                query = query.Where(x => x.PaymentNumber.Contains(searchParams.PaymentNumber));

            if (!string.IsNullOrWhiteSpace(searchParams.PaymentDocumentNumber))
                query = query.Where(x => x.PaymentCNumberBVU.Contains(searchParams.PaymentDocumentNumber));

            if (searchParams.PaymentStatusId != null)
                query = query.Where(x => x.PaymentStatus.Id == searchParams.PaymentStatusId);

            query = query.Where(x => x.IsForeignCurrency == searchParams.IsForeignCurrency);

            if (!string.IsNullOrWhiteSpace(searchParams.CurrencyCode))
                query = query.Where(x => x.CurrencyType.Contains(searchParams.CurrencyCode));

            return query;
        }
    }
}
