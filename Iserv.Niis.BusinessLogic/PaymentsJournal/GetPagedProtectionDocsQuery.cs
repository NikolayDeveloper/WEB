using Iserv.Niis.BusinessLogic.PaymentsJournal.Dto;
using Iserv.Niis.BusinessLogic.PaymentsJournal.Interfaces;
using Iserv.Niis.Infrastructure.Pagination;
using Microsoft.AspNetCore.Http;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; ;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Queries;

namespace Iserv.Niis.BusinessLogic.PaymentsJournal
{
    public class GetPagedProtectionDocsQuery : BaseQuery, IBasePaymentsJournalQuery<IPagedList<DocumentDto>>
    {
        private readonly IExecutor _executor;
        public GetPagedProtectionDocsQuery(IExecutor executor)
        {
            _executor = executor;
        }

        public IPagedList<DocumentDto> Execute(DocumentsSearchParametersDto searchParameters, HttpRequest httpRequest)
        {
            var protectionDocs = _executor.GetQuery<GetProtectionDocsQuery>().Process(q => q.Execute(searchParameters, httpRequest));

            return protectionDocs.ToPagedList(httpRequest.GetPaginationParams());
        }
    }
}