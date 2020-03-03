using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Iserv.Niis.Domain.Entities.Payment;
using Microsoft.EntityFrameworkCore;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Queries;

namespace Iserv.Niis.BusinessLogic.PaymentInvoices
{
    public class GetPaymentInvoicesByContractIdQuery : BaseQuery
    {
        public async Task<List<PaymentInvoice>> ExecuteAsync(int contractId)
        {
            var paymentInvoiceRepository = Uow.GetRepository<PaymentInvoice>();
            var paymentInvoices = await paymentInvoiceRepository
                .AsQueryable()
                .Include(pi => pi.Contract).ThenInclude(c => c.ProtectionDocType)
                .Include(pi => pi.PaymentUses)
                .Include(pi => pi.Tariff)
                .Include(pi => pi.Status)
                .Where(pi => pi.ContractId == contractId)
                .OrderByDescending(pi => pi.DateCreate)
                .ToListAsync();
            return paymentInvoices;
        }
    }
}