using AutoMapper;
using Iserv.Niis.BusinessLogic.Security;
using Iserv.Niis.DI;
using Iserv.Niis.Domain.Entities.Dictionaries;
using Iserv.Niis.Domain.Entities.Payment;
using Iserv.Niis.Model.Models.Payment;
using Microsoft.EntityFrameworkCore;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; ;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Commands;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Iserv.Niis.BusinessLogic.Payments
{
    public class PaymentReturnAmountCommand : BaseCommand
    {
        private readonly IExecutor _executor;
        private readonly IMapper _mapper;

        public PaymentReturnAmountCommand(IExecutor executor, IMapper mapper)
        {
            _executor = executor;
            _mapper = mapper;
        }

        public async Task<PaymentReturnAmountResponseDto> ExecuteAsync(int paymentId, PaymentReturnAmountRequestDto requestDto)
        {
            var responseDto = new PaymentReturnAmountResponseDto();

            var paymentRepository = Uow.GetRepository<Payment>();
            var paymentUseRepository = Uow.GetRepository<PaymentUse>();
            var paymentStatusRepository = Uow.GetRepository<DicPaymentStatus>();

            var user = _executor.GetQuery<GetUserByIdQuery>()
                .Process(q => q.Execute(NiisAmbientContext.Current.User.Identity.UserId));

            var payment = await paymentRepository
                .AsQueryable()
                .Include(p => p.PaymentUses)
                .FirstOrDefaultAsync(p => p.Id == paymentId);

            var paymentDto = _mapper.Map<Payment, PaymentDto>(payment);

            responseDto.PaymentUsesExist = paymentUseRepository
                .AsQueryable()
                .Any(pu => pu.PaymentId == paymentId && !pu.IsDeleted);

            var paymentReminderAmountRnd = decimal.Round(paymentDto.RemainderAmount, 2);
            var returnAmountRnd = decimal.Round(requestDto.ReturnAmount, 2);

            if (requestDto.ReturnFullAmount)
            {
                if (!responseDto.PaymentUsesExist)
                {
                    payment.ReturnedAmount = payment.Amount;
                    payment.ReturnedDate = DateTimeOffset.Now;
                    payment.EmployeeNameReturnedPayment = $"{user.NameRu} {user.Position?.NameRu}";
                    payment.ReturnedReason = requestDto.ReturnReason;
                    paymentRepository.Update(payment);
                    Uow.SaveChanges();

                    responseDto.Success = true;
                }
            }
            else
            {
                if (returnAmountRnd <= paymentReminderAmountRnd + payment.ReturnedAmount.GetValueOrDefault())
                {
                    payment.ReturnedAmount = returnAmountRnd;
                    payment.ReturnedDate = DateTimeOffset.Now;
                    payment.EmployeeNameReturnedPayment = $"{user.NameRu} {user.Position?.NameRu}";
                    payment.ReturnedReason = requestDto.ReturnReason;
                    paymentRepository.Update(payment);
                    Uow.SaveChanges();

                    responseDto.Success = true;
                }
                else
                {
                    responseDto.ReturnAmountIsGreaterThanPaymentReminder = true;
                }
            }

            if (responseDto.Success)
            {
                _executor.GetCommand<UpdatePaymentStatusCommand>()
                    .Process(c => c.Execute(payment.Id));
            }

            return responseDto;
        }
    }
}