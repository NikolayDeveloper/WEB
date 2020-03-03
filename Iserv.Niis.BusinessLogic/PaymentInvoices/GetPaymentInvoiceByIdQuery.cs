using System.Threading.Tasks;
using Iserv.Niis.Domain.Entities.Payment;
using Microsoft.EntityFrameworkCore;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Queries;

namespace Iserv.Niis.BusinessLogic.PaymentInvoices
{
    public class GetPaymentInvoiceByIdQuery : BaseQuery
    {
        /// <summary>
        /// Выполнение запроса.
        /// </summary>
        /// <param name="paymentInvoiceId">Идентификатор выставленного счета.</param>
        /// <returns>Выставленный счет.</returns>
        public async Task<PaymentInvoice> ExecuteAsync(int paymentInvoiceId)
        {
            var paymentInvoiceRepository = Uow.GetRepository<PaymentInvoice>();
            var paymentInvoice = paymentInvoiceRepository
                .AsQueryable()
                .Include(pi => pi.Request).ThenInclude(r => r.ProtectionDocType)
				.Include(pi => pi.Request).ThenInclude(r => r.RequestType)
				.Include(pi => pi.Request).ThenInclude(r => r.Contracts)
				.Include(pi => pi.Request).ThenInclude(r => r.Documents)
				.Include(pi => pi.Request).ThenInclude(r => r.RequestCustomers)
				.Include(pi => pi.Request).ThenInclude(r => r.RequestCustomers).ThenInclude(c => c.CustomerRole)
				.Include(pi => pi.Request).ThenInclude(r => r.RequestCustomers).ThenInclude(c => c.Customer).ThenInclude(c => c.Type)
				.Include(pi => pi.Request).ThenInclude(r => r.RequestCustomers).ThenInclude(c => c.Customer).ThenInclude(c => c.BeneficiaryType)
				.Include(pi => pi.Request).ThenInclude(r => r.RequestCustomers).ThenInclude(c => c.Customer).ThenInclude(c => c.Country)
				.Include(pi => pi.Request).ThenInclude(r => r.Department)
				.Include(pi => pi.ProtectionDoc).ThenInclude(pd => pd.Type)
				.Include(pi => pi.ProtectionDoc).ThenInclude(pd => pd.ProtectionDocCustomers)
				.Include(pi => pi.ProtectionDoc).ThenInclude(pd => pd.ProtectionDocCustomers).ThenInclude(c => c.CustomerRole)
				.Include(pi => pi.ProtectionDoc).ThenInclude(pd => pd.ProtectionDocCustomers).ThenInclude(c => c.Customer).ThenInclude(c => c.Type)
				.Include(pi => pi.ProtectionDoc).ThenInclude(pd => pd.ProtectionDocCustomers).ThenInclude(c => c.Customer).ThenInclude(c => c.BeneficiaryType)
				.Include(pi => pi.ProtectionDoc).ThenInclude(pd => pd.ProtectionDocCustomers).ThenInclude(c => c.Customer).ThenInclude(c => c.Country)				
				.Include(pi => pi.Contract).ThenInclude(c => c.ProtectionDocType)
				.Include(pi => pi.Contract).ThenInclude(c => c.ContractCustomers).ThenInclude(c => c.CustomerRole)
				.Include(pi => pi.Contract).ThenInclude(c => c.ContractCustomers).ThenInclude(c => c.Customer).ThenInclude(c => c.Type)
				.Include(pi => pi.Contract).ThenInclude(c => c.ContractCustomers).ThenInclude(c => c.Customer).ThenInclude(c => c.BeneficiaryType)
				.Include(pi => pi.Contract).ThenInclude(c => c.ContractCustomers).ThenInclude(c => c.Customer).ThenInclude(c => c.Country)				
                .Include(pi => pi.PaymentUses)
                .Include(pi => pi.Tariff)
                .Include(pi => pi.Status)
				.Include(pi => pi.WhoBoundUser)
				.Include(pi => pi.WriteOffUser)
				.Include(pi => pi.CreateUser)
				
				.FirstOrDefaultAsync(p => p.Id == paymentInvoiceId);
            return await paymentInvoice;
        }
    }
}