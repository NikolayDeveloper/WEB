using System.Threading.Tasks;
using Iserv.Niis.Domain.Entities.Payment;
using Microsoft.EntityFrameworkCore;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Queries;

namespace Iserv.Niis.BusinessLogic.Payments
{
	public class GetPaymentByIdQuery : BaseQuery
	{
		public async Task<Payment> ExecuteAsync(int paymentId)
		{
			var paymentRepository = Uow.GetRepository<Payment>();
			var payment = paymentRepository
				.AsQueryable()
				.Include(pi => pi.PaymentUses)
				.Include(r => r.Customer)
				.FirstOrDefaultAsync(p => p.Id == paymentId);
			return await payment;
		}
	}
}