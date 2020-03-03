using System.ComponentModel;
using System.Linq;
using Iserv.Niis.BusinessLogic.Excel;
using Iserv.Niis.BusinessLogic.PaymentsJournal;
using Iserv.Niis.BusinessLogic.PaymentsJournal.Dto;
using Iserv.Niis.BusinessLogic.PaymentsJournal.Interfaces;
using Iserv.Niis.DataBridge.Implementations;
using Iserv.Niis.Infrastructure.Pagination;
using Microsoft.AspNetCore.Mvc;

namespace Iserv.Niis.Api.Controllers.PaymentsJournal
{
    [Produces("application/json")]
    [Route("api/PaymentsJournal/Documents")]
    public class DocumentsController : BaseNiisApiController
    {
        /// <summary>
        /// Получает постраничный список документов.
        /// </summary>
        /// <param name="searchParameters">Параметры поиска.</param>
        /// <returns>Постраничный список документов.</returns>
        [HttpGet]
        public IActionResult Get([FromQuery]DocumentsSearchParametersDto searchParameters)
        {
            var result = GetDocumentsBySearchParameters<GetPagedRequestsQuery, GetPagedProtectionDocsQuery,
                GetPagedContractsQuery, IPagedList<DocumentDto>>(searchParameters);

            return result.AsOkObjectResult(this.Response);
        }

        /// <summary>
        /// Получает Excel файл.
        /// </summary>
        /// <param name="searchParameters">Параметры поиска.</param>
        /// <returns>Excel файл.</returns>
        [HttpGet("GetExcel")]
        public IActionResult GetExcel([FromQuery]DocumentsSearchParametersDto searchParameters)
        {
            var result = GetDocumentsBySearchParameters<GetRequestsQuery, GetProtectionDocsQuery,
                GetContractsQuery, IQueryable<DocumentDto>>(searchParameters).ToList();

            var fileStream = Executor.GetCommand<GetExcelFileCommand>().Process(q => q.Execute(result, Request));
            return File(fileStream, GetExcelFileCommand.ContentType, GetExcelFileCommand.DefaultFileName);
        }

        /// <summary>
        /// Получает документы с учетом параметров поиска.
        /// </summary>
        /// <typeparam name="TGetRequestsQuery">Тип запроса получающего заявки.</typeparam>
        /// <typeparam name="TGetProtectionDocsQuery">Тип запроса получающего охранные документы.</typeparam>
        /// <typeparam name="TGetContractsQuery">Тип запроса получающего контракты.</typeparam>
        /// <typeparam name="TResult">Тип результирующего объекта.</typeparam>
        /// <param name="searchParameters">Параметры поиска.</param>
        /// <returns>Результирующий объект.</returns>
        private TResult GetDocumentsBySearchParameters<TGetRequestsQuery, TGetProtectionDocsQuery, TGetContractsQuery, TResult>(DocumentsSearchParametersDto searchParameters)
            where TGetRequestsQuery: BaseCommand, IBasePaymentsJournalQuery<TResult> 
            where TGetProtectionDocsQuery : BaseCommand, IBasePaymentsJournalQuery<TResult>
			where TGetContractsQuery : BaseCommand, IBasePaymentsJournalQuery<TResult>
			where TResult : class
        {
            switch (searchParameters.DocumentCategory)
            {
                case DocumentCategory.Request:
                    return Executor.GetQuery<TGetRequestsQuery>().Process(q => q.Execute(searchParameters, Request));

                case DocumentCategory.ProtectionDoc:
                    return Executor.GetQuery<TGetProtectionDocsQuery>().Process(q => q.Execute(searchParameters, Request));

                case DocumentCategory.Contract:
                    return Executor.GetQuery<TGetContractsQuery>().Process(q => q.Execute(searchParameters, Request));

                default:
                    throw new InvalidEnumArgumentException(nameof(searchParameters.DocumentCategory),
                        (int) searchParameters.DocumentCategory, typeof(DocumentCategory));
            }
        }
    }
}