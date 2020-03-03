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
using System.Threading.Tasks;

namespace Iserv.Niis.BusinessLogic.Payments
{
    public class PaymentBlockAmountCommand : BaseCommand
    {
        private readonly IExecutor _executor;
        private readonly IMapper _mapper;

        public PaymentBlockAmountCommand(IExecutor executor, IMapper mapper)
        {
            _executor = executor;
            _mapper = mapper;
        }

        public async Task<PaymentBlockAmountResponseDto> ExecuteAsync(int paymentId, PaymentBlockAmountRequestDto requestDto)
        {
            var responseDto = new PaymentBlockAmountResponseDto();

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

            var paymentReminderAmountRnd = decimal.Round(paymentDto.RemainderAmount, 2);
            var blockAmountRnd = decimal.Round(requestDto.BlockAmount, 2);

            if (blockAmountRnd <= paymentReminderAmountRnd + payment.BlockedAmount.GetValueOrDefault())
            {
                payment.BlockedAmount = blockAmountRnd;
                payment.BlockedDate = DateTimeOffset.Now;
                payment.EmployeeNameBlockedPayment = $"{user.NameRu} {user.Position?.NameRu}";
                payment.BlockedReason = requestDto.BlockReason;
                paymentRepository.Update(payment);
                Uow.SaveChanges();

                responseDto.Success = true;
            }
            else
            {
                responseDto.BlockAmountIsGreaterThanPaymentReminder = true;
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