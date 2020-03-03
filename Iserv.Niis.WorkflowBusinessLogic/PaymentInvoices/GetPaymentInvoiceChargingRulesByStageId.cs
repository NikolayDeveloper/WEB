using Iserv.Niis.Domain.Entities.Rules;
using Microsoft.EntityFrameworkCore;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Queries;
using System.Linq;

namespace Iserv.Niis.WorkflowBusinessLogic.PaymentInvoices
{
    /// <summary>
    /// Запрос возвращающий массив правил автосписывания платежей для этапа маршрута по его идентификатору.
    /// </summary>
    public class GetPaymentInvoiceChargingRulesByStageId : BaseQuery
    {
        /// <summary>
        /// Выполнение запроса.
        /// </summary>
        /// <param name="stageId">Идентификатор этапа маршрута.</param>
        /// <param name="nextStageId">Идентификатор следующего этапа запроса.</param>
        /// <returns>Массив правил автосписывания платежей.</returns>
        public PaymentInvoiceChargingRule[] Execute(int stageId, int nextStageId)
        {
            var ruleRepository = Uow.GetRepository<PaymentInvoiceChargingRule>();

            return ruleRepository.AsQueryable()
                .Include(rule => rule.Tariff)
                .Where(rule => !rule.Tariff.IsDeleted)
                .Where(rule => rule.StageId == stageId)
                .Where(rule => rule.NextStageId == nextStageId)
                .ToArray();
        } 
    }
}
