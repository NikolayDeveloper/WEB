using Iserv.Niis.BusinessLogic.PaymentsJournal.Dto;
using Iserv.Niis.BusinessLogic.PaymentsJournal.Interfaces;
using Iserv.Niis.Infrastructure.Pagination;
using Microsoft.AspNetCore.Http;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; ;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Queries;

namespace Iserv.Niis.BusinessLogic.PaymentsJournal
{
    public class GetPagedContractsQuery : BaseQuery, IBasePaymentsJournalQuery<IPagedList<DocumentDto>>
    {
        private readonly IExecutor _executor;
        public GetPagedContractsQuery(IExecutor executor)
        {
            _executor = executor;
        }

        public IPagedList<DocumentDto> Execute(DocumentsSearchParametersDto searchParameters, HttpRequest httpRequest)
        {
            var contracts = _executor.GetQuery<GetContractsQuery>().Process(q => q.Execute(searchParameters, httpRequest));

            return contracts.ToPagedList(httpRequest.GetPaginationParams());
        }
    }
}