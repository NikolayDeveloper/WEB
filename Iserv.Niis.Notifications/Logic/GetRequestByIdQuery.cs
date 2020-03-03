using System.Linq;
using Iserv.Niis.Domain.Entities.Request;
using Microsoft.EntityFrameworkCore;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Queries;

namespace Iserv.Niis.Notifications.Logic
{
    public class GetRequestByIdQuery : BaseQuery
    {
        public Request Execute(int requestId)
        {
            var repository = Uow.GetRepository<Request>();
            return repository.AsQueryable().Include(rd => rd.CurrentWorkflow)
                .ThenInclude(cw => cw.FromStage)
                .ThenInclude(fs => fs.OnlineRequisitionStatus)
                .Include(r => r.CurrentWorkflow)
                .ThenInclude(cw => cw.CurrentStage)
                .ThenInclude(сs => сs.OnlineRequisitionStatus)
                .Include(r => r.Addressee)
                .ThenInclude(c => c.ContactInfos)
                .ThenInclude(ci => ci.Type)
                .Include(r => r.Addressee)
                .ThenInclude(c => c.ContactInfos)
                .SingleOrDefault(r => r.Id == requestId);
        }
    }
}
