using Iserv.Niis.Domain.Entities.Request;
using Microsoft.EntityFrameworkCore;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Queries;
using System.Collections.Generic;
using System.Linq;

namespace Iserv.Niis.Documents.DocumentsBusinessLogic.Requests
{
    public class GetRequestsByDocumentIdQuery : BaseQuery
    {
        public List<Request> Execute(int documentId)
        {
            var repo = Uow.GetRepository<RequestDocument>();

            var result = repo.AsQueryable()
                .Where(rd => rd.DocumentId == documentId)
                .Select(rd => rd.Request)
                .Include(r => r.RequestCustomers).ThenInclude(rc => rc.Customer)
                .Include(r => r.RequestCustomers).ThenInclude(rc => rc.CustomerRole)
                .Include(r => r.ProtectionDocType)
                .ToList();

            return result;
        }
    }
}
