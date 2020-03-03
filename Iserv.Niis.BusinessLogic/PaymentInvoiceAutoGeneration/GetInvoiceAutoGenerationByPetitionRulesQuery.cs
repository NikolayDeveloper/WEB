using System.Linq;
using Iserv.Niis.Domain.Entities.AutoPaymentInvoiceGeneration;
using Microsoft.EntityFrameworkCore;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Queries;

namespace Iserv.Niis.BusinessLogic.PaymentInvoiceAutoGeneration
{
    public class GetInvoiceAutoGenerationByPetitionRulesQuery: BaseQuery
    {
        public IQueryable<PaymentInvoiceGenerationByPetitionRule> Execute()
        {
            var repo = Uow.GetRepository<PaymentInvoiceGenerationByPetitionRule>();

            return repo.AsQueryable()
                .Include(r => r.Tariff)
                .AsQueryable();
        }
    }
}
