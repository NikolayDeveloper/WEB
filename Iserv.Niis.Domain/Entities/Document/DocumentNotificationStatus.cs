using Iserv.Niis.Domain.Entities.Dictionaries;

namespace Iserv.Niis.Domain.Entities.Document
{
    public class DocumentNotificationStatus
    {
        public int DocumentId { get; set; }
        public Document Document { get; set; }
        public int NotificationStatusId { get; set; }
        public DicNotificationStatus NotificationStatus { get; set; }
    }
}
