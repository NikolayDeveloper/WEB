using Iserv.Niis.Common.Codes;
using Iserv.Niis.WorkflowBusinessLogic.SystemCounter;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Handlers;
using System;
using System.Linq;
using System.Threading.Tasks;
using Iserv.Niis.Domain.Entities.Dictionaries;

namespace Iserv.Niis.BusinessLogic.Documents.Numbers
{
    public class GenerateNumberForPaymentHandler : BaseHandler
    {
        public async Task ExecuteAsync(int documentId)
        {
            var document = await Executor.GetQuery<GetDocumentByIdQuery>().Process(q => q.ExecuteAsync(documentId));
            var paymentInvoiceTypeCodes = new[]
            {
                DicDocumentTypeCodes.NotificationOfTmRequestReviewingAcceptance,
                DicDocumentTypeCodes.NotificationOfRegistrationDecision,
                DicDocumentTypeCodes.DecisionNotification
            };

            if (!paymentInvoiceTypeCodes.Contains(document.Type.Code))
                return;

            var count = Executor.GetHandler<GetNextCountHandler>().Process(h => h.Execute("OutgoingNumberForPayment"));
            document.NumberForPayment = count.ToString();
            document.PaymentDate = DateTimeOffset.Now;

            await Executor.GetCommand<UpdateDocumentCommand>().Process(c => c.Execute(document));
        }
    }
}
