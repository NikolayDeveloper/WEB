using Iserv.Niis.Domain.Entities.Request;
using NetCoreCQRS.Queries;

namespace Iserv.Niis.Notifications.Logic
{
    public class DeleteRequestNotificationStatusCommand : BaseQuery
    {
        public void Execute(RequestNotificationStatus requestNotificationStatus)
        {
            Uow.GetRepository<RequestNotificationStatus>().Delete(requestNotificationStatus);
            Uow.SaveChanges();
        }
    }
}
