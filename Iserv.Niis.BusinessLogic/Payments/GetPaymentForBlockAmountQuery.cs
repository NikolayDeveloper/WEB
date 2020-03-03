using AutoMapper;
using Iserv.Niis.Domain.Entities.Payment;
using Iserv.Niis.Model.Models.Payment;
using Microsoft.EntityFrameworkCore;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Queries;
using System.Threading.Tasks;

namespace Iserv.Niis.BusinessLogic.Payments
{
    public class GetPaymentForBlockAmountQuery : BaseQuery
    {
        private readonly IMapper _mapper;

        public GetPaymentForBlockAmountQuery(IMapper mapper)
        {
            _mapper = mapper;
        }

        public async Task<GetPaymentForBlockAmountResponseDto> ExecuteAsync(int paymentId)
        {
            var responseDto = new GetPaymentForBlockAmountResponseDto();

            var paymentRepository = Uow.GetRepository<Payment>();

            var payment = await paymentRepository
                .AsQueryable()
                .Include(p => p.PaymentUses)
                .FirstOrDefaultAsync(p => p.Id == paymentId);

            var paymentDto = _mapper.Map<Payment, PaymentDto>(payment);

            responseDto.Id = payment.Id;
            responseDto.PaymentReminder = paymentDto.RemainderAmount + payment.BlockedAmount.GetValueOrDefault();

            return responseDto;
        }
    }
}