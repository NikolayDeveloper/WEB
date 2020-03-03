using Iserv.Niis.Domain.Entities.Request;
using Microsoft.EntityFrameworkCore;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Queries;
using System.Linq;

namespace Iserv.Niis.WorkflowBusinessLogic.RequestDocuments
{
    public class GetLastRequestsDocumentQuery : BaseQuery
    {
        public RequestDocument Execute(int requestId, params string[] typeCode)
        {
            var requestDocumentRepository = Uow.GetRepository<RequestDocument>();
            var requestDocumentQuery = requestDocumentRepository
                .AsQueryable()
                .Include(r => r.Document)
                .Where(r => r.RequestId == requestId);

            if (typeCode.Any())
            {
                requestDocumentQuery = requestDocumentQuery.Where(r => 
                                        r.Document != null 
                                        && r.Document.Type != null
                                        && typeCode.Contains(r.Document.Type.Code));
            }

            return requestDocumentQuery.LastOrDefault();
        }
    }
}