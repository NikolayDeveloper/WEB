using System.Linq;
using Iserv.Niis.Domain.Entities.Request;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Queries;

namespace Iserv.Niis.Notifications.Logic
{
    public class GetRequestNotificationStatusQuery : BaseQuery
    {
        public RequestNotificationStatus Execute(int requestId)
        {
            return Uow.GetRepository<RequestNotificationStatus>().AsQueryable()
                .SingleOrDefault(rn => rn.RequestId == requestId);
        }
    }
}
