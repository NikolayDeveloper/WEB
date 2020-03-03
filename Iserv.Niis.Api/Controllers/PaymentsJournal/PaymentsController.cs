using Iserv.Niis.BusinessLogic.Excel;
using Iserv.Niis.BusinessLogic.PaymentsJournal;
using Iserv.Niis.BusinessLogic.PaymentsJournal.Dto;
using Iserv.Niis.Infrastructure.Pagination;
using Microsoft.AspNetCore.Mvc;

namespace Iserv.Niis.Api.Controllers.PaymentsJournal
{
    [Produces("application/json")]
    [Route("api/PaymentsJournal/Payments")]
    public class PaymentsController : BaseNiisApiController
    {
        /// <summary>
        /// Возвращает пагинированный, отсортированный 
        /// и отфильтрованный список с платежами.
        /// </summary>
        /// <param name="searchParameters">Параметры поиска.</param>
        /// <returns>Список с платежами.</returns>
        [HttpGet]
        public IActionResult Get([FromQuery]PaymentsSearchParametersDto searchParameters)
        {
            var pagedPaymentJournalDto = Executor
                .GetQuery<GetPagedPaymentsQuery>()
                .Process(q => q.Execute(searchParameters, Request));

            return pagedPaymentJournalDto.AsOkObjectResult(Response);
        }

        [HttpGet("GetExcel")]
        public IActionResult GetExcel([FromQuery]PaymentsSearchParametersDto searchParameters)
        {
            var payments = Executor.GetQuery<GetPaymentsQuery>().Process(q => q.Execute(searchParameters, Request));

            var fileStream = Executor.GetCommand<GetExcelFileCommand>().Process(q => q.Execute(payments, Request));
            return File(fileStream, GetExcelFileCommand.ContentType, GetExcelFileCommand.DefaultFileName);
        }

        /// <summary>
        /// Возвращает ответ со списком валют.
        /// </summary>
        /// <returns>Ответ со списком валют.</returns>
        [HttpGet("currencies")]
        public IActionResult GetCurrencies()
        {
            var result = Executor
                .GetQuery<GetCurrenciesQuery>()
                .Process(q => q.Execute());

            return Ok(result);
        }
    }
}