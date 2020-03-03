using Iserv.Niis.BusinessLogic.PaymentsJournal.Dto;
using Iserv.Niis.BusinessLogic.PaymentsJournal.Interfaces;
using Iserv.Niis.Infrastructure.Pagination;
using Microsoft.AspNetCore.Http;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; ;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Queries;

namespace Iserv.Niis.BusinessLogic.PaymentsJournal
{
    public class GetPagedRequestsQuery : BaseQuery, IBasePaymentsJournalQuery<IPagedList<DocumentDto>>
    {
        private readonly IExecutor _executor;
        public GetPagedRequestsQuery(IExecutor executor)
        {
            _executor = executor;
        }

        public IPagedList<DocumentDto> Execute(DocumentsSearchParametersDto searchParameters, HttpRequest httpRequest)
        {
            var requests = _executor.GetQuery<GetRequestsQuery>().Process(q => q.Execute(searchParameters, httpRequest));

            return requests.ToPagedList(httpRequest.GetPaginationParams());
        }
    }
}