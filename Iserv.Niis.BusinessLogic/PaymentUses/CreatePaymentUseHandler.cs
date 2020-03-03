using Iserv.Niis.BusinessLogic.PaymentInvoices;
using Iserv.Niis.BusinessLogic.Payments;
using Iserv.Niis.Domain.Entities.Payment;
using Iserv.Niis.Domain.Helpers;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Handlers;

namespace Iserv.Niis.BusinessLogic.PaymentUses
{
    public class CreatePaymentUseHandler : BaseHandler
    {
        public void Execute(Owner.Type ownerType, PaymentUse paymentUse, bool force)
        {
            Executor.CommandChain()
                .AddCommand<CreditPaymentInvoiceCommand>(c => c.Execute(ownerType, paymentUse, force))
                .AddCommand<CreatePaymentUseCommand>(c => c.Execute(paymentUse))
                .ExecuteAllWithTransaction();

            Executor.GetCommand<UpdatePaymentStatusCommand>()
                .Process(c => c.Execute(paymentUse.PaymentId.Value));
        }
    }
}