using Iserv.Niis.Domain.Entities.Payment;
using Iserv.Niis.BusinessLogic.PaymentsJournal.Dto;
using Microsoft.EntityFrameworkCore;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Queries;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Iserv.Niis.BusinessLogic.Payments
{
	public class GetPaymentsByInvoiceId : BaseQuery
    {
		public async Task<List<PaymentDto>> ExecuteAsync(int paymentInvoiceId)
        {
			var paymentUseRepository = Uow.GetRepository<PaymentUse>();
			var paymentUses = paymentUseRepository
				.AsQueryable()
				.Where(r => r.PaymentInvoiceId == paymentInvoiceId)				
				.ToList();
			
			var paymentRepository = Uow.GetRepository<Payment>();
			
			var payments = paymentRepository
                .AsQueryable()
                .AsNoTracking()
				.Include(r => r.Customer)
				.Include(r => r.PaymentStatus)
				.Include(r => r.PaymentUses)                
				.Where(p=> paymentUses.Select(u => u.PaymentId).Contains(p.Id))
				.Select(PaymentDto.FromPaymentDto)				
				.OrderByDescending(i=>i.Id)
				.ToListAsync();

			foreach(var item in payments.Result)
			{
				item.Distributed = paymentUses.Where(u=> !u.IsDeleted && u.PaymentId == item.Id ).Sum(u=>u.Amount);
			}
			
			return await payments;
        }
    }
}
