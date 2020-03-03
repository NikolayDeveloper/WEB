using System.Linq;
using Iserv.Niis.Domain.Entities.AutoPaymentInvoiceGeneration;
using Microsoft.EntityFrameworkCore;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Queries;

namespace Iserv.Niis.BusinessLogic.PaymentInvoiceAutoGeneration
{
    public class GetPaymentInvoiceGenerationRulesQuery: BaseQuery
    {
        public IQueryable<PaymentInvoiceGenerationRule> Execute()
        {
            var repo = Uow.GetRepository<PaymentInvoiceGenerationRule>();

            return repo.AsQueryable()
                .Include(r => r.Tariff)
                .AsQueryable();
        }
    }
}
