using System.Linq;
using Iserv.Niis.Domain.Entities.Payment;
using Microsoft.EntityFrameworkCore;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Queries;

namespace Iserv.Niis.WorkflowBusinessLogic.PaymentInvoices
{
    public class GetPaymentInvoiceByIdQuery: BaseQuery
    {
        public PaymentInvoice Execute(int id)
        {
            var repo = Uow.GetRepository<PaymentInvoice>();

            return repo.AsQueryable()
                .Include(r => r.PaymentUses)
                .Include(r => r.Tariff)
                .SingleOrDefault(r => r.Id == id);
        }
    }
}
