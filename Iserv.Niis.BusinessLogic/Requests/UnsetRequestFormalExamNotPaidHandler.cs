using System.Linq;
using System.Threading.Tasks;
using Iserv.Niis.Common.Codes;
using Iserv.Niis.Domain.Entities.Payment;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Handlers;

namespace Iserv.Niis.BusinessLogic.Requests
{
    /// <summary>
    /// Снятие флага, что оплата за предварительную экспертизу не оплачена в срок
    /// </summary>
    public class UnsetRequestFormalExamNotPaidHandler: BaseHandler
    {
        public async Task ExecuteAsync(int requestId, PaymentInvoice paymentInvoice)
        {
            //коды тарифов оплаты за предварительную экспертизу
            var formalExamTariffCodes = new[]
            {
                DicTariffCodes.CollectiveTmFormalExpertizeDigital,
                DicTariffCodes.CollectiveTmFormalExpertizePaper,
                DicTariffCodes.TmNmptFormalExpertizeDigital,
                DicTariffCodes.TmNmptFormalExpertizePaper
            };
            if (formalExamTariffCodes.Contains(paymentInvoice?.Tariff?.Code) && paymentInvoice?.Status.Code == DicPaymentStatusCodes.Credited)
            {
                //снятие флага с заявки
                var request = await Executor.GetQuery<GetRequestByIdQuery>()
                    .Process(q => q.ExecuteAsync(requestId));
                request.IsFormalExamFeeNotPaidInTime = false;
                Executor.GetCommand<WorkflowBusinessLogic.Requests.UpdateRequestCommand>().Process(c => c.Execute(request));
            }
        }
    }
}
