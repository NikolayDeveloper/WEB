using Iserv.Niis.Domain.Entities.Payment;
using Iserv.Niis.Model.Models.Payment;
using Microsoft.EntityFrameworkCore;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Queries;
using System.Threading.Tasks;

namespace Iserv.Niis.BusinessLogic.PaymentUses
{
    public class GetPaymentUseForEditQuery : BaseQuery
    {
        public async Task<GetPaymentUseForEditResponseDto> ExecuteAsync(int paymentUseId)
        {
            var paymentUseRepository = Uow.GetRepository<PaymentUse>();

            var paymentUse = await paymentUseRepository
                .AsQueryable()
                .Include(pu => pu.Payment)
                .FirstOrDefaultAsync(pu => pu.Id == paymentUseId);

            var paymentUseForEdit = new GetPaymentUseForEditResponseDto
            {
                Id = paymentUse.Id,
                Amount = paymentUse.Amount                
            };

            return paymentUseForEdit;
        }
    }
}