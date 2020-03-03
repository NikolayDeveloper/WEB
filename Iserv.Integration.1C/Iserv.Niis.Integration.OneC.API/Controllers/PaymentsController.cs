using System;
using System.Collections.Generic;
using System.Net;
using Iserv.Niis.Integration.ApiResult.Models;
using Iserv.Niis.Integration.OneC.Infrastructure.Queries;
using Iserv.Niis.Integration.OneC.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Iserv.Niis.Integration.OneC.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    [Authorize]
    public class PaymentsController : BaseApiController
    {
        /// <summary>
        /// Получает платежи за платежи в диапазоне дат "с" .. "по".
        /// </summary>
        /// <param name="fromDate">Дата "с".</param>
        /// <param name="toDate">Дата "по".</param>
        /// <returns>Результат выполнения API.</returns>
        [HttpGet(nameof(GetPaymentsByDateRange))]
        [ProducesResponseType(typeof(GetDataApiResult<IEnumerable<PaymentFrom1CDto>>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(GetDataApiResult<IEnumerable<PaymentFrom1CDto>>), (int)HttpStatusCode.InternalServerError)]
        public IActionResult GetPaymentsByDateRange(DateTime fromDate, DateTime toDate)
        {
            try
            {
                var result = Executor.GetQuery<GetPaymentsByDateRangeQuery>().Process(q => q.Execute(fromDate, toDate));
                return Ok(result);
            }
            catch (Exception e)
            {
                return InternalServerError<GetDataApiResult<IEnumerable<PaymentFrom1CDto>>>(e);
            }
        }
    }
}
