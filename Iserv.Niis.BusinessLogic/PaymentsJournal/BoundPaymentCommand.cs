using System;
using System.Linq;
using Iserv.Niis.BusinessLogic.PaymentsJournal.Dto;
using Iserv.Niis.DI;
using Iserv.Niis.Domain.Entities.Contract;
using Iserv.Niis.Domain.Entities.Payment;
using Iserv.Niis.Domain.Entities.ProtectionDoc;
using Iserv.Niis.Domain.Entities.Request;
using Iserv.Niis.Domain.Entities.Security;
using Microsoft.EntityFrameworkCore;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Commands;

namespace Iserv.Niis.BusinessLogic.PaymentsJournal
{
    public class BoundPaymentCommand : BaseCommand
    {
        public void Execute(BoundPaymentDto boundPaymentDto)
        {
            var payment = GetPayment(boundPaymentDto.PaymentId);
            var paymentInvoice = GetPaymentInvoice(boundPaymentDto.PaymentInvoiceId);
            var currentUser = this.GetCurrentUser();

            var paymentUse = new PaymentUse
            {
                Payment = payment,
                PaymentInvoice = paymentInvoice,
                Request = paymentInvoice.Request,
                ProtectionDoc = paymentInvoice.ProtectionDoc,
                Contract = paymentInvoice.Contract,
                Amount = boundPaymentDto.Amount,
                Description = boundPaymentDto.Description,
                DateCreate = DateTimeOffset.Now,
                EmployeeCheckoutPaymentName = currentUser.NameRu
            };

            payment.PaymentUseAmmountSumm = payment.PaymentUses.Sum(x => x.Amount) + paymentUse.Amount;
            Uow.GetRepository<Payment>().Update(paymentUse.Payment);

            Uow.GetRepository<PaymentUse>().Create(paymentUse);
            Uow.SaveChanges();
        }

        private Payment GetPayment(int id)
        {
            var payment = Uow.GetRepository<Payment>().AsQueryable().Include(x => x.PaymentUses).FirstOrDefault(x => x.Id == id);

            if (payment == null)
            {
                throw new Exception($"Платеж с ИД {id} не найден.");
            }

            return payment;
        }

        private PaymentInvoice GetPaymentInvoice(int id)
        {
            var paymentInvoice = Uow.GetRepository<PaymentInvoice>().AsQueryable()
                .Include(x => x.Request)
                .Include(x => x.ProtectionDoc)
                .Include(x => x.Contract)
                .FirstOrDefault(x => x.Id == id);

            if (paymentInvoice == null)
            {
                throw new Exception($"Выставленная оплата с ИД {id} не найдена.");
            }

            return paymentInvoice;
        }

        private ApplicationUser GetCurrentUser()
        {
            var id = NiisAmbientContext.Current.User.Identity.UserId;

            var user = Uow.GetRepository<ApplicationUser>().GetById(id);

            if (user == null)
            {
                throw new Exception($"Пользователь с ИД {id} не найден.");
            }

            return user;
        }
    }
}