using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Iserv.Niis.Domain.Entities.Workflow;
using Iserv.Niis.Utils.Helpers;
using Microsoft.EntityFrameworkCore;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Queries;

namespace Iserv.Niis.WorkflowBusinessLogic.WorkflowTaskQueues
{
    /// <summary>
    /// Класс, который представляет запрос, возвращающий список невыполненных запланированных задач, у которых подошло время выполнения.
    /// <para></para>
    /// Внимание, возвращаются только те задачи, которые связаны с заявкой, охранным документом (ОД) или контрактом.
    /// </summary>

    public class GetWorkflowQueueByPeriodQuery : BaseQuery
    {
        /// <summary>
        /// Выполнение запроса.
        /// </summary>
        /// <param name="periodStart">Начало периода времени.</param>
        /// <param name="periodEnd">Конец периода времени.</param>
        /// <returns>Список запланированных задач.</returns>
        public List<WorkflowTaskQueue> Execute(DateTimeOffset periodStart, DateTimeOffset periodEnd)
        {
            var workflowTaskQueueRepository = Uow.GetRepository<WorkflowTaskQueue>();

            var workflowTaskQueues = workflowTaskQueueRepository
                .AsQueryable()
                .IgnoreQueryFilters()
                    .Where(IsTaskForRequestOrProtectionDocOrContract())
                    .Where(IsTaskMissedOrResolveDateInPeriod(periodStart, periodEnd))
                    .Where(r => r.IsExecuted != true)
                .ToList();

            return workflowTaskQueues;
        }

        /// <summary>
        /// Возвращает выражение, которое проверяет задачу на то, является ли она задачей на заявку, охранный документ (ОД) или контракт.
        /// </summary>
        /// <returns>Выражение для проверки задачи.</returns>
        private static Expression<Func<WorkflowTaskQueue, bool>> IsTaskForRequestOrProtectionDocOrContract()
        {
            return queue => queue.RequestId.HasValue || queue.ProtectionDocId.HasValue || queue.ContractId.HasValue;
        }

        /// <summary>
        /// Возвращает выражение, которое проверяет задачу на то, находится ли время его выполнения между указанным промежутком времени. <para></para>
        /// Время сравнивается включительно.
        /// </summary>
        /// <param name="periodStart">Начало периода времени.</param>
        /// <param name="periodEnd">Конец периода времени.</param>
        /// <returns>Выражение для проверки задачи.</returns>
        private static Expression<Func<WorkflowTaskQueue, bool>> IsTaskResolveDateInPeriod(DateTimeOffset periodStart, DateTimeOffset periodEnd)
        {
            return queue => queue.ResolveDate >= periodStart && queue.ResolveDate <= periodEnd;
        }

        /// <summary>
        /// Возвращает выражение, которое проверяет задачу на то, пропущена ли она.
        /// </summary>
        /// <param name="periodStart">Время с которым сравнивается время, но которое было запланировано выполнение задачи.</param>
        /// <returns>Выражение для проверки задачи</returns>
        private static Expression<Func<WorkflowTaskQueue, bool>> IsTaskMissed(DateTimeOffset periodStart)
        {
            return queue => queue.ResolveDate < periodStart && queue.IsExecuted != true;
        }

        /// <summary>
        /// Возвращает совмещенные выражения <see cref="IsTaskMissed(DateTimeOffset)"/> м <see cref="IsTaskResolveDateInPeriod(DateTimeOffset, DateTimeOffset)"/>
        /// </summary>
        /// <param name="periodStart">Начало периода времени.</param>
        /// <param name="periodEnd">Конец периода времени.</param>
        /// <returns>Выражение для проверки задачи.</returns>
        private static Expression<Func<WorkflowTaskQueue, bool>> IsTaskMissedOrResolveDateInPeriod(DateTimeOffset periodStart, DateTimeOffset periodEnd)
        {
            var inPeriodExpression = IsTaskResolveDateInPeriod(periodStart, periodEnd);
            var isMissedExpression = IsTaskMissed(periodStart);

            return ExpressionHelper.CombineExpressions(inPeriodExpression, isMissedExpression, Expression.Or);    
        }
    }
}