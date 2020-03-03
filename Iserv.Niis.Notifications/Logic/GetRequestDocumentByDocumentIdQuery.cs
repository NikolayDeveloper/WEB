using System.Linq;
using Iserv.Niis.Domain.Entities.Request;
using Microsoft.EntityFrameworkCore;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Queries;

namespace Iserv.Niis.Notifications.Logic
{
    public class GetRequestDocumentByDocumentIdQuery : BaseQuery
    {
        public RequestDocument Execute(int documentId)
        {
            var repository = Uow.GetRepository<RequestDocument>();
            return repository.AsQueryable().Include(rd => rd.Request)
                .ThenInclude(r => r.CurrentWorkflow)
                .ThenInclude(rw => rw.CurrentStage).ThenInclude(rs => rs.OnlineRequisitionStatus)
                .FirstOrDefault(r => r.DocumentId == documentId);
        }
    }
}
