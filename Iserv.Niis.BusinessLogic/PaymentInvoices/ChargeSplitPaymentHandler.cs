using System.Linq;
using System.Threading.Tasks;
using Iserv.Niis.BusinessLogic.Dictionaries.DicPaymentStatuses;
using Iserv.Niis.BusinessLogic.Requests;
using Iserv.Niis.Common.Codes;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Handlers;

namespace Iserv.Niis.BusinessLogic.PaymentInvoices
{
    public class ChargeSplitPaymentHandler: BaseHandler
    {
        public async Task ExecuteAsync(int childRequestId, string documentTypeCode)
        {
            var notificationCodes = new[]
            {
                DicDocumentTypeCodes.NotificationReleaseRequestRegistrationNotification,
                DicDocumentTypeCodes.ResponseReleaseRequestRegistrationNotification
            };
            if (!notificationCodes.Contains(documentTypeCode))
                return;

            var parents = await Executor.GetQuery<GetParentRequestsByChildRequestIdQuery>()
                .Process(q => q.Execute(childRequestId));
            foreach (var request in parents)
            {
                var paymentInvoice = request.PaymentInvoices.FirstOrDefault(pi =>
                    pi.Tariff.Code == DicTariffCodes.TmSplit && pi.Status.Code == DicPaymentStatusCodes.Credited);
                var chargedStatus = Executor.GetQuery<GetDicPaymentStatusByCodeQuery>()
                    .Process(q => q.Execute(DicPaymentStatusCodes.Charged));
                if (paymentInvoice != null)
                {
                    paymentInvoice.Status = chargedStatus;
                    paymentInvoice.StatusId = chargedStatus.Id;
                    Executor.GetCommand<UpdatePaymentInvoiceCommand>().Process(c => c.Execute(paymentInvoice));
                }
            }
        }
    }
}
