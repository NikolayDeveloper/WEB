using System.Linq;
using System.Threading.Tasks;
using Iserv.Niis.Common.Codes;
using Iserv.Niis.Domain.Entities.Payment;
using Iserv.Niis.Domain.Enums;
using Iserv.Niis.WorkflowBusinessLogic.System;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Handlers;

namespace Iserv.Niis.BusinessLogic.Requests
{
    public class IsIcgspaidForHandler: BaseHandler
    {
        public async Task<bool> Execute(int requestId)
        {
            var fullExpertizeTariffCodes = new[]
            {
                DicTariffCodes.CollectiveTmNmptFullExpertizeDigital,
                DicTariffCodes.CollectiveTmNmptFullExpertizePaper,
                DicTariffCodes.TmNmptFullExpertizeDigital,
                DicTariffCodes.TmNmptFullExpertizePaper
            };
            var fullExpertizeTariffCodesForMoreThanThreeClasses = new[]
            {
                DicTariffCodes.TmNmptFullExpertizeMoreThanThreeIcgsClassesPaper,
                DicTariffCodes.TmNmptFullExpertizeMoreThanThreeIcgsClassesDigital,
                DicTariffCodes.CollectiveTmNmptFullExpertizeMoreThanThreeIcgsClassesDigital,
                DicTariffCodes.CollectiveTmNmptFullExpertizeMoreThanThreeIcgsClassesPaper,
            };
            var formalExpertizeTariffCodesForMoreThanThreeClasses = new[]
           {
                DicTariffCodes.TmNmptFormalExpertizeMoreThanThreeIcgsClassesPaper,
                DicTariffCodes.TmNmptFormalExpertizeMoreThanThreeIcgsClassesDigital,
                DicTariffCodes.CollectiveTmFormalExpertizeMoreThanThreeIcgsClassesPaper,
                DicTariffCodes.CollectiveTmFormalExpertizeMoreThanThreeIcgsClassesDigital,
            };

            var request = await Executor.GetQuery<GetRequestByIdQuery>().Process(q => q.ExecuteAsync(requestId));
            var icgsCount = request.ICGSRequests.Count;
            if (!int.TryParse(
                Executor.GetQuery<GetSystemSettingsByTypeQuery>()
                    .Process(q => q.Execute(SettingType.IcgsCountThreshold)), out var icgsThreshold))
            {
                icgsThreshold = 3;
            }
            if (icgsCount <= icgsThreshold)
            {
                return request.PaymentInvoices.Any(pi =>
                    fullExpertizeTariffCodes.Contains(pi.Tariff.Code) &&
                    pi.Status.Code == DicPaymentStatusCodes.Credited);
            }
            var tariffCount = icgsCount - icgsThreshold;

            var totalFullTariffCount = request.PaymentInvoices.Where(pi =>
                fullExpertizeTariffCodesForMoreThanThreeClasses.Contains(pi.Tariff.Code) &&
                (IsUnderPaid(pi, 100) || pi.Status.Code == DicPaymentStatusCodes.Credited))
                .Sum(pi => pi.TariffCount);

            var totalFormalTariffCount = request.PaymentInvoices.Where(pi =>
                formalExpertizeTariffCodesForMoreThanThreeClasses.Contains(pi.Tariff.Code) &&
                (IsUnderPaid(pi, 100) || pi.Status.Code == DicPaymentStatusCodes.Credited))
                .Sum(pi => pi.TariffCount);

            if (totalFullTariffCount != tariffCount || totalFormalTariffCount != tariffCount) return false;

            return totalFullTariffCount + totalFormalTariffCount >= tariffCount;
        }

        private bool IsUnderPaid(PaymentInvoice invoice, decimal underpaymentAmount)
        {
            return invoice.Tariff.Price * (decimal)0.12 + invoice.Tariff.Price <= invoice.PaymentUses.Sum(pu => pu.Amount) + underpaymentAmount &&
                   invoice.Status.Code == DicPaymentStatusCodes.Notpaid;
        }
    }
}
