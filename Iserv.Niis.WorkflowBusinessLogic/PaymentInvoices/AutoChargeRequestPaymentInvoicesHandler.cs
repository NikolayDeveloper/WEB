using Iserv.Niis.Common.Codes;
using Iserv.Niis.Documents.DocumentsBusinessLogic.Requests;
using Iserv.Niis.Domain.Entities.Payment;
using Iserv.Niis.WorkflowBusinessLogic.Security;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Handlers;
using System;
using System.Linq;

namespace Iserv.Niis.WorkflowBusinessLogic.PaymentInvoices
{
    public class AutoChargeRequestPaymentInvoicesHandler : BaseHandler
    {
        /// <summary>
        /// Выполнение обработчика.
        /// </summary>
        /// <param name="requestId">Идентификатор заявки.</param>
        /// <param name="fromStageId">Идентификатор текущего этапа заявки.</param>
        /// <param name="nextStageId">Идентификатор следующего этапа заявки.</param>
        /// <returns>Были ли обновлены данные.</returns>
        public bool Execute(int requestId, int fromStageId, int nextStageId)
        {
            //var paymentInvoicesToCharge = GetPaymentInvoicesToCharge(requestId, fromStageId, nextStageId);

            //if (!paymentInvoicesToCharge.Any())
            //{
            //    return false;
            //}

            //var chargedStatus = Executor.GetQuery<GetPaymentStatusByCodeQuery>()
            //                                .Process(query => query.Execute(DicPaymentStatusCodes.Charged));

            //var systemUser = Executor.GetQuery<GetUserByXinQuery>()
            //                                .Process(query => query.Execute(UserConstants.SystemUserXin));

            //var chargeDate = DateTimeOffset.Now;

            //foreach(var paymentInvoice in paymentInvoicesToCharge)
            //{
            //    paymentInvoice.StatusId = chargedStatus.Id;
            //    paymentInvoice.DateComplete = chargeDate;
            //    paymentInvoice.WriteOffUserId = systemUser.Id;
            //}

            //Executor.GetCommand<UpdatePaymentInvoicesCommand>()
            //    .Process(command => command.Execute(paymentInvoicesToCharge));

            return true;
        }

        /// <summary>
        /// Возвращает массив платежей, которые нужно отметить как списанные.
        /// </summary>
        /// <param name="requestId">Идентификатор заявки.</param>
        /// <param name="fromStageId">Идентификатор текущего этапа маршрута.</param>
        /// <param name="nextStageId">Идентификатор следующего этапа маршрута.</param>
        /// <returns>Массив платежей.</returns>
        private PaymentInvoice[] GetPaymentInvoicesToCharge(int requestId, int fromStageId, int nextStageId)
        {
            var request = Executor.GetQuery<GetRequestByIdQuery>()
                                      .Process(query => query.Execute(requestId));

            var rules = Executor.GetQuery<GetPaymentInvoiceChargingRulesByStageId>()
                .Process(query => query.Execute(fromStageId, nextStageId));

            var tariffIds = rules.Select(rule => rule.TariffId).ToArray();

            return request.PaymentInvoices
                            .Where(IsCredited())
                            .Where(IsPaymentTarrifInGivenTariffs(tariffIds))
                            .ToArray();
        }

        /// <summary>
        /// Возвращает делегает, который проверяет, является ли платеж зачтенным.
        /// </summary>
        /// <returns>Делегат.</returns>
        private static Func<PaymentInvoice, bool> IsCredited()
        {
            return paymentInvoice => 
                paymentInvoice.Status != null &&
                paymentInvoice.Status.Code !=null &&
                paymentInvoice.Status.Code == DicPaymentStatusCodes.Credited;
        }
        /// <summary>
        /// Возвращает делегает, который проверяет, находится ли тариф платежа в массиве заданых
        /// тарифов.
        /// </summary>
        /// <param name="tariffIds">Массив идентификаторов тарифов.</param>
        /// <returns>Делегат.</returns>
        private static Func<PaymentInvoice, bool> IsPaymentTarrifInGivenTariffs(int[] tariffIds)
        {
            return paymentInvoice =>
                paymentInvoice.Tariff != null &&
                tariffIds.Contains(paymentInvoice.Tariff.Id);
        }
    }
}
