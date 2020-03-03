using Iserv.Niis.Domain.Entities.Payment;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Commands;
using System.Collections.Generic;

namespace Iserv.Niis.WorkflowBusinessLogic.PaymentInvoices
{
    /// <summary>
    /// Обновляет данные о платежах.
    /// </summary>
    public class UpdatePaymentInvoicesCommand : BaseCommand
    {
        /// <summary>
        /// Выполнение запроса.
        /// </summary>
        /// <param name="paymentInvoices">Платежи.</param>
        /// <returns>Асинхронная операция.</returns>
        public void Execute(IEnumerable<PaymentInvoice> paymentInvoices)
        {
            var paymentInvoiceRepository = Uow.GetRepository<PaymentInvoice>();
            paymentInvoiceRepository.UpdateRange(paymentInvoices);        
            Uow.SaveChanges();
        }
    }
}
