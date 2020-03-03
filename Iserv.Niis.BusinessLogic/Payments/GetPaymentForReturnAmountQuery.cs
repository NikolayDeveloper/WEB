using AutoMapper;
using Iserv.Niis.Domain.Entities.Payment;
using Iserv.Niis.Model.Models.Payment;
using Microsoft.EntityFrameworkCore;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Queries;
using System.Threading.Tasks;

namespace Iserv.Niis.BusinessLogic.Payments
{
    public class GetPaymentForReturnAmountQuery : BaseQuery
    {
        private readonly IMapper _mapper;

        public GetPaymentForReturnAmountQuery(IMapper mapper)
        {
            _mapper = mapper;
        }

        public async Task<GetPaymentForReturnAmountResponseDto> ExecuteAsync(int paymentId)
        {
            var responseDto = new GetPaymentForReturnAmountResponseDto();

            var paymentRepository = Uow.GetRepository<Payment>();

            var payment = await paymentRepository
                .AsQueryable()
                .Include(p => p.PaymentUses)
                .FirstOrDefaultAsync(p => p.Id == paymentId);

            var paymentDto = _mapper.Map<Payment, PaymentDto>(payment);

            responseDto.Id = payment.Id;
            responseDto.PaymentReminder = paymentDto.RemainderAmount + payment.ReturnedAmount.GetValueOrDefault();

            return responseDto;
        }
    }
}