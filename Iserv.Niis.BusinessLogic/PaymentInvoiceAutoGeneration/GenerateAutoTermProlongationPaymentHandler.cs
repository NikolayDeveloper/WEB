using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Iserv.Niis.BusinessLogic.Documents;
using Iserv.Niis.BusinessLogic.Security;
using Iserv.Niis.Common.Codes;
using Iserv.Niis.Domain.Helpers;
using Iserv.Niis.WorkflowBusinessLogic.PaymentInvoices;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Handlers;

namespace Iserv.Niis.BusinessLogic.PaymentInvoiceAutoGeneration
{
    public class GenerateAutoTermProlongationPaymentHandler: BaseHandler
    {
        private readonly List<ProlongationTermTariff> _tariffs = new List<ProlongationTermTariff>
        {
            new ProlongationTermTariff
            {
                MonthNumber = 1,
                TariffCode = DicTariffCodes.ResponseTimeProlongationFirstMonth
            },
            new ProlongationTermTariff
            {
                MonthNumber = 2,
                TariffCode = DicTariffCodes.ResponseTimeProlongationSecondMonth
            },
            new ProlongationTermTariff
            {
                MonthNumber = 3,
                TariffCode = DicTariffCodes.ResponseTimeProlongationThirdMonth
            },
            new ProlongationTermTariff
            {
                MonthNumber = 4,
                TariffCode = DicTariffCodes.ResponseTimeProlongationFourthMonth
            },
            new ProlongationTermTariff
            {
                MonthNumber = 5,
                TariffCode = DicTariffCodes.ResponseTimeProlongationFifthMonth
            },
            new ProlongationTermTariff
            {
                MonthNumber = 6,
                TariffCode = DicTariffCodes.ResponseTimeProlongationSixthMonth
            },
        };

        //public async Task ExecuteAsync(int requestId, int documentId)
        //{
        //    var petition = await Executor.GetQuery<GetDocumentByIdQuery>().Process(q => q.ExecuteAsync(documentId));
        //    var tariffs = _tariffs.Where(t => t.MonthNumber <= petition.AttachedPaymentsCount)
        //        .Select(t => t.TariffCode);
        //    var systemUser = Executor.GetQuery<GetUserByXinQuery>()
        //        .Process(q => q.Execute(UserConstants.SystemUserXin));
        //    foreach (var tariffCode in tariffs)
        //    {
        //        Executor.GetHandler<CreatePaymentInvoiceHandler>().Process(h =>
        //            h.Execute(Owner.Type.Request, requestId, tariffCode, DicPaymentStatusCodes.Notpaid, systemUser?.Id));
        //    }
        //}
    }

    internal class ProlongationTermTariff
    {
        internal int MonthNumber { get; set; }
        internal string TariffCode { get; set; }
    }
}
