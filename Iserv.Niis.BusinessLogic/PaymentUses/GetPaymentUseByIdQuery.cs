using Iserv.Niis.Domain.Entities.Payment;
using Microsoft.EntityFrameworkCore;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Queries;
using System.Threading.Tasks;

namespace Iserv.Niis.BusinessLogic.PaymentUses
{
    public class GetPaymentUseByIdQuery : BaseQuery
    {
        public async Task<PaymentUse> ExecuteAsync(int paymentUseId)
        {
            var paymentUseRepository = Uow.GetRepository<PaymentUse>();
            var paymentUse = await paymentUseRepository.AsQueryable()
                .Include(u => u.PaymentInvoice)
                .FirstOrDefaultAsync(u => u.Id == paymentUseId);

            return paymentUse;
        }
    }
}