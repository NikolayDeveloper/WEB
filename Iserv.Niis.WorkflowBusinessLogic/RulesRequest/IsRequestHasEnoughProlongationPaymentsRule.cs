using System.Collections.Generic;
using System.Linq;
using Iserv.Niis.Common.Codes;
using Iserv.Niis.WorkflowBusinessLogic.Document;
using Iserv.Niis.WorkflowBusinessLogic.Requests;
using Iserv.Niis.WorkflowBusinessLogic.WorkflowRequests;
using NetCoreRules;

namespace Iserv.Niis.WorkflowBusinessLogic.RulesRequest
{
    public class IsRequestHasEnoughProlongationPaymentsRule: BaseRule<RequestWorkFlowRequest>
    {
        public bool Eval(string documentTypeCode)
        {
            var request = Executor.GetQuery<GetRequestByIdQuery>().Process(q => q.Execute(WorkflowRequest.RequestId));
            var tariffs = new List<TariffMonthNumber>
            {
                new TariffMonthNumber {TariffCode = DicTariffCodes.ResponseTimeProlongationFirstMonth, MonthNumber = 1},
                new TariffMonthNumber {TariffCode = DicTariffCodes.ResponseTimeProlongationSecondMonth, MonthNumber = 2},
                new TariffMonthNumber {TariffCode = DicTariffCodes.ResponseTimeProlongationThirdMonth, MonthNumber = 3},
                new TariffMonthNumber {TariffCode = DicTariffCodes.ResponseTimeProlongationFourthMonth, MonthNumber = 4},
                new TariffMonthNumber {TariffCode = DicTariffCodes.ResponseTimeProlongationFifthMonth, MonthNumber = 5},
                new TariffMonthNumber {TariffCode = DicTariffCodes.ResponseTimeProlongationSixthMonth, MonthNumber = 6}
            };
            var document = Executor.GetQuery<GetDocumentsByRequestIdAndTypeCodesQuery>().Process(q =>
                q.Execute(WorkflowRequest.RequestId, new[] { documentTypeCode }))
                .FirstOrDefault(d => d.DateCreate >= request.CurrentWorkflow.DateCreate);
            var requiredTariffCodes = tariffs.Where(t => t.MonthNumber <= document?.AttachedPaymentsCount)
                .Select(t => t.TariffCode);

            if (!requiredTariffCodes.Any())
                return false;

            return requiredTariffCodes.All(rtc => request.PaymentInvoices.Select(pi => pi.Tariff.Code).Contains(rtc));
        }

        private class TariffMonthNumber
        {
            internal string TariffCode { get; set; }
            internal int MonthNumber { get; set; }
        }
    }
}
