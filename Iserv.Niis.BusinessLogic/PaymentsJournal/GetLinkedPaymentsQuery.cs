using System.Collections.Generic;
using System.Linq;
using Iserv.Niis.BusinessLogic.PaymentsJournal.Dto;
using Iserv.Niis.Domain.Entities.Payment;
using Microsoft.EntityFrameworkCore;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Queries;

namespace Iserv.Niis.BusinessLogic.PaymentsJournal
{
    public class GetLinkedPaymentsQuery : BaseQuery
    {
        public IList<LinkedPaymentDto> Execute(int paymentInvoiceId)
        {
            return GetQuery(paymentInvoiceId)
                .Select(LinkedPaymentDto.FromPaymentUse)
                .ToList();
        }

        private IQueryable<PaymentUse> GetQuery(int paymentInvoiceId)
        {
            var query = Uow.GetRepository<PaymentUse>()
                .AsQueryable()
                .AsNoTracking()
                .Include(r => r.Payment).ThenInclude(r => r.Customer)
                .Include(x => x.Payment).ThenInclude(x => x.PaymentStatus);

            return query.Where(x => x.PaymentInvoiceId == paymentInvoiceId);
        }
    }
}