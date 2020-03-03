using System.Linq;
using System.Threading.Tasks;
using Iserv.Niis.DataBridge.Implementations;
using Iserv.Niis.Domain.Entities.Request;
using Microsoft.EntityFrameworkCore;
//using NetCoreCQRS.Queries;

namespace Iserv.Niis.BusinessLogic.Requests
{
    public class GetRequestByDocumentIdQuery : BaseQuery
    {
        public async Task<Request> ExecuteAsync(int documentId)
        {
            var repo = Uow.GetRepository<Request>();
            var request = repo
                .AsQueryable()
                .Include(r => r.ProtectionDocType)
                .FirstOrDefault(r => Enumerable.Any<RequestDocument>(r.Documents, d => d.DocumentId == documentId));

            return request;
        }
    }
}
