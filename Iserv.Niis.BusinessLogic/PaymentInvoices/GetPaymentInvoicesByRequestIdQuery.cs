using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Iserv.Niis.Domain.Entities.Payment;
using Microsoft.EntityFrameworkCore;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Queries;

namespace Iserv.Niis.BusinessLogic.PaymentInvoices
{
    /// <summary>
    /// Запрос для получения списка выставленных счетов по идентификатору заявки.
    /// </summary>
    public class GetPaymentInvoicesByRequestIdQuery : BaseQuery
    {
        /// <summary>
        /// Выполнение запроса.
        /// </summary>
        /// <param name="requestId">Идентификатор заявки.</param>
        /// <returns>Список выставленных счетов.</returns>
        public async Task<List<PaymentInvoice>> ExecuteAsync(int requestId)
        {
            var paymentInvoiceRepository = Uow.GetRepository<PaymentInvoice>();
            var paymentInvoices = await paymentInvoiceRepository
                .AsQueryable()
                .Include(pi => pi.Request).ThenInclude(c => c.ProtectionDocType)
                .Include(pi => pi.PaymentUses)
                .Include(pi => pi.Tariff)
                .Include(pi => pi.Status)
                .Include(pi => pi.WhoBoundUser)
                .Include(pi => pi.WriteOffUser)
                .Include(pi => pi.CreateUser)
                .Where(pi => pi.RequestId == requestId)
                .OrderByDescending(pi => pi.DateCreate)
                .ToListAsync();
            return paymentInvoices;
        }
    }
}