using System.Collections.Generic;
using System.Linq;
using Iserv.Niis.Domain.Entities.Payment;
using Microsoft.EntityFrameworkCore;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Queries;

namespace Iserv.Niis.BusinessLogic.PaymentInvoices
{
    /// <summary>
    /// Запрос для получения списка выставленных счетов по их идентификаторам.
    /// </summary>
    public class GetPaymentInvoicesByIdsQuery : BaseQuery
    {
        /// <summary>
        /// Выполнение запроса.
        /// </summary>
        /// <param name="paymentInvoiceIds">Массив идентификаторов счетов.</param>
        /// <returns>Список выставленных счетов.</returns>
        public List<PaymentInvoice> Execute(int[] paymentInvoiceIds)
        {
            var paymentInvoiceRepository = Uow.GetRepository<PaymentInvoice>();
            var paymentInvoices = paymentInvoiceRepository
                .AsQueryable()
                .Include(pi => pi.Request).ThenInclude(r => r.ProtectionDocType)
                .Include(pi => pi.ProtectionDoc).ThenInclude(pd => pd.Type)
                .Include(pi => pi.Contract).ThenInclude(c => c.ProtectionDocType)
                .Include(pi => pi.PaymentUses)
                .Include(pi => pi.Tariff)
                .Include(pi => pi.Status)
                .Where(p => paymentInvoiceIds.Contains(p.Id))
                .ToList();
            return paymentInvoices;
        }
    }
}