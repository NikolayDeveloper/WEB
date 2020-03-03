using Iserv.Niis.Domain.Entities.Payment;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Commands;

namespace Iserv.Niis.BusinessLogic.PaymentInvoices
{
    /// <summary>
    /// Запрос на обновление данных о выставленном счете.
    /// </summary>
    public class UpdatePaymentInvoiceCommand: BaseCommand
    {
        /// <summary>
        /// Выполнение запроса.
        /// </summary>
        /// <param name="paymentInvoice">Выставленный счет.</param>
        public void Execute(PaymentInvoice paymentInvoice)
        {
            var paymentInvoiceRepository = Uow.GetRepository<PaymentInvoice>();
            paymentInvoiceRepository.Update(paymentInvoice);
            Uow.SaveChanges();
        }
    }
}
