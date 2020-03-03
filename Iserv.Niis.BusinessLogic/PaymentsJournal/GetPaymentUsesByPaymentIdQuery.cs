using System.Collections.Generic;
using System.Linq;
using Iserv.Niis.Domain.Entities.Payment;
using Iserv.Niis.Model.Models.PaymentsJournal;
using Microsoft.EntityFrameworkCore;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Queries;

namespace Iserv.Niis.BusinessLogic.PaymentsJournal
{
    /// <summary>
    /// Получает распределенные оплаты по идентификатору платежа.
    /// </summary>
    public class GetPaymentUsesByPaymentIdQuery : BaseQuery
    {
        /// <summary>
        /// Выполнение запроса.
        /// </summary>
        /// <param name="paymentId">Идентификатор платежа.</param>
        /// <returns>Лист с распределенными оплатами.</returns>
        public IList<PaymentUseDto> Execute(int paymentId)
        {
            return GetQuery(paymentId)
                .Select(PaymentUseDto.FromPaymentUse)
                .ToList();
        }

        private IQueryable<PaymentUse> GetQuery(int paymentId)
        {
            var query = Uow.GetRepository<PaymentUse>()
                .AsQueryable()
                .AsNoTracking()
                .Include(r => r.Payment).ThenInclude(r => r.Customer)
                .Include(r => r.PaymentInvoice).ThenInclude(r => r.Tariff)
                .Include(r => r.PaymentInvoice).ThenInclude(x => x.Request).ThenInclude(r => r.ProtectionDocType)
                .Include(r => r.PaymentInvoice).ThenInclude(x => x.ProtectionDoc).ThenInclude(r => r.Type)
                .Include(r => r.PaymentInvoice).ThenInclude(x => x.Contract).ThenInclude(r => r.ProtectionDocType)
                .Include(r => r.PaymentInvoice).ThenInclude(x => x.Contract).ThenInclude(r => r.Type);

            return query.Where(x => x.PaymentId == paymentId);
        }
    }
}