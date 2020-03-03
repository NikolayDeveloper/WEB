using System.Linq;
using System.Threading.Tasks;
using Iserv.Niis.Domain.Entities.Payment;
using Iserv.Niis.Model.Models.PaymentsJournal;
using Microsoft.EntityFrameworkCore;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Queries;
using NetCoreDataAccess.Repository;

namespace Iserv.Niis.BusinessLogic.PaymentsJournal
{
    public class GetPaymentUseByIdQuery : BaseQuery
    {
        public async Task<PaymentUseDto> ExecuteAsync(int paymentUseId)
        {
            IRepository<PaymentUse> paymentUseRepository = Uow.GetRepository<PaymentUse>();

            return await paymentUseRepository
                .AsQueryable()
                .AsNoTracking()
                .Include(paymentUse => paymentUse.Payment)
                .ThenInclude(payment => payment.Customer)
                .Include(paymentUse => paymentUse.PaymentInvoice)
                .ThenInclude(paymentInvoice => paymentInvoice.Tariff)
                .Include(paymentUse => paymentUse.PaymentInvoice)
                .ThenInclude(paymentInvoice => paymentInvoice.Request)
                .ThenInclude(request => request.ProtectionDocType)
                .Include(paymentUse => paymentUse.PaymentInvoice)
                .ThenInclude(paymentInvoice => paymentInvoice.ProtectionDoc)
                .ThenInclude(protectionDoc => protectionDoc.Type)
                .Include(paymentUse => paymentUse.PaymentInvoice)
                .ThenInclude(paymentInvoice => paymentInvoice.Contract)
                .ThenInclude(contract => contract.ProtectionDocType)
                .Include(paymentUse => paymentUse.PaymentInvoice)
                .ThenInclude(paymentInvoice => paymentInvoice.Contract)
                .ThenInclude(contract => contract.Type)
                .Select(PaymentUseDto.FromPaymentUse)
                .FirstOrDefaultAsync(paymentUse => paymentUse.Id == paymentUseId);
        }
    }
}
