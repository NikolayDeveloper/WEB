using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Iserv.Niis.Domain.Entities.Payment;
using Microsoft.EntityFrameworkCore;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Queries;

namespace Iserv.Niis.BusinessLogic.PaymentInvoices
{
	public class GetPaymentsByIdsQuery : BaseQuery
	{
		public async Task<List<Payment>> ExecuteAsync(int[] paymentIds)
		{
			var paymentRepository = Uow.GetRepository<Payment>();
			var payments = paymentRepository
				.AsQueryable()
				.Include(pi => pi.PaymentUses)
				.Include(r => r.Customer)
				.Where(p => paymentIds.Contains(p.Id))
				.ToListAsync();
			return await payments;
		}
	}
}