using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Iserv.Niis.Integration.ApiResult.Models;
using Iserv.Niis.Integration.OneC.Model;

namespace Iserv.Niis.Business.Abstract
{
    /// <summary>
    /// Интерфейс клиента для интеграции с 1C.
    /// </summary>
    public interface IIntegrationOneCApiClient
    {
        /// <summary>
        /// Получает платежи в диапазоне дат "с" .. "по".
        /// </summary>
        /// <param name="fromDate">Дата "с".</param>
        /// <param name="toDate">Дата "по".</param>
        /// <returns>Результат выполнения API.</returns>
        Task<GetDataApiResult<List<PaymentFrom1CDto>>> GetPaymentsByDateRange(DateTimeOffset fromDate, DateTimeOffset toDate);
    }
}