using AutoMapper;
using Iserv.Niis.Common.Codes;
using Iserv.Niis.Domain.Entities.Dictionaries;
using Iserv.Niis.Domain.Entities.Payment;
using Iserv.Niis.Model.Models.Payment;
using Microsoft.EntityFrameworkCore;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Commands;
using System.Linq;

namespace Iserv.Niis.BusinessLogic.Payments
{
    public class UpdatePaymentStatusCommand : BaseCommand
    {
        private readonly IMapper _mapper;

        public UpdatePaymentStatusCommand(IMapper mapper)
        {
            _mapper = mapper;
        }

        public void Execute(int paymentId)
        {
            var paymentRepository = Uow.GetRepository<Payment>();
            var paymentStatusRepository = Uow.GetRepository<DicPaymentStatus>();

            var payment = paymentRepository
                .AsQueryable()
                .Include(p => p.PaymentUses)
                .First(p => p.Id == paymentId);

            var paymentDto = _mapper.Map<Payment, PaymentDto>(payment);

            if (payment.Amount.GetValueOrDefault() == paymentDto.PaymentUseAmountSum)
            {
                payment.PaymentStatus = paymentStatusRepository
                    .AsQueryable()
                    .First(q => q.Code == DicPaymentStatusCodes.Distributed);
            }
            else if (payment.Amount.GetValueOrDefault() == payment.ReturnedAmount.GetValueOrDefault())
            {
                payment.PaymentStatus = paymentStatusRepository
                    .AsQueryable()
                    .First(q => q.Code == DicPaymentStatusCodes.Returned);
            }
            else
            {
                payment.PaymentStatus = paymentStatusRepository
                    .AsQueryable()
                    .First(q => q.Code == DicPaymentStatusCodes.NotDistributed);
            }

            if (payment.ReturnedAmount.GetValueOrDefault() == 0)
            {
                payment.ReturnedAmount = null;
                payment.ReturnedDate = null;
                payment.EmployeeNameReturnedPayment = null;
            }

            if (payment.BlockedAmount.GetValueOrDefault() == 0)
            {
                payment.BlockedAmount = null;
                payment.BlockedDate = null;
                payment.EmployeeNameBlockedPayment = null;
            }

            paymentRepository.Update(payment);
            Uow.SaveChanges();
        }
    }
}