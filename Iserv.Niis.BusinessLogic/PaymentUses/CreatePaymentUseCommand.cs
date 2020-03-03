using System;
using Iserv.Niis.BusinessLogic.PaymentInvoices;
using Iserv.Niis.DI;
using Iserv.Niis.Domain.Entities.Payment;
using Iserv.Niis.Domain.Entities.Security;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; ;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Commands;

namespace Iserv.Niis.BusinessLogic.PaymentUses
{
    public class CreatePaymentUseCommand : BaseCommand
    {
        private readonly IExecutor _executor;

        public CreatePaymentUseCommand(IExecutor executor)
        {
            _executor = executor;
        }

        public int Execute(PaymentUse paymentUse)
        {
            if (paymentUse.PaymentInvoiceId != null)
            {
                var paymentInvoice = _executor.GetQuery<GetPaymentInvoiceByIdQuery>()
                    .Process(q => q.ExecuteAsync(paymentUse.PaymentInvoiceId.Value)).Result;

                paymentUse.RequestId = paymentInvoice.Request?.Id;
                paymentUse.ProtectionDocId = paymentInvoice.ProtectionDoc?.Id;
                paymentUse.ContractId = paymentInvoice.Contract?.Id;
            }

            paymentUse.DateOfPayment = DateTimeOffset.Now;
            paymentUse.EmployeeCheckoutPaymentName = Uow.GetRepository<ApplicationUser>().GetById(NiisAmbientContext.Current.User.Identity.UserId)?.NameRu;
            var paymentUseRepository = Uow.GetRepository<PaymentUse>();
            paymentUseRepository.Create(paymentUse);
            Uow.SaveChanges();
            return paymentUse.Id;
        }

        public int ExecuteSystem(PaymentUse paymentUse)
        {
            if (paymentUse.PaymentInvoiceId != null)
            {
                var paymentInvoice = _executor.GetQuery<GetPaymentInvoiceByIdQuery>()
                    .Process(q => q.ExecuteAsync(paymentUse.PaymentInvoiceId.Value)).Result;

                paymentUse.RequestId = paymentInvoice.Request?.Id;
                paymentUse.ProtectionDocId = paymentInvoice.ProtectionDoc?.Id;
                paymentUse.ContractId = paymentInvoice.Contract?.Id;
            }

            paymentUse.DateOfPayment = DateTimeOffset.Now;
            paymentUse.EmployeeCheckoutPaymentName = "Автоимпорт";
            var paymentUseRepository = Uow.GetRepository<PaymentUse>();
            paymentUseRepository.Create(paymentUse);
            Uow.SaveChanges();
            return paymentUse.Id;
        }
    }
}