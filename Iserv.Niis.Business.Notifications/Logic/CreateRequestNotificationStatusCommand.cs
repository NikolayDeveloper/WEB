using Iserv.Niis.Domain.Entities.Request;
using NetCoreCQRS.Commands;

namespace Iserv.Niis.Notifications.Logic
{
    public class CreateRequestNotificationStatusCommand : BaseCommand
    {
        public void Execute(RequestNotificationStatus requestNotificationStatus)
        {
            Uow.GetRepository<RequestNotificationStatus>().Create(requestNotificationStatus);
            Uow.SaveChanges();
        }
    }
}
