using Iserv.Niis.Domain.Entities.Dictionaries;
using Iserv.Niis.Domain.Entities.Document;

namespace Iserv.Niis.Domain.Entities.Request
{
    public class RequestNotificationStatus
    {
        public int RequestId { get; set; }
        public Request Request { get; set; }
        public int NotificationStatusId { get; set; }
        public DicNotificationStatus NotificationStatus { get; set; }
    }
}
