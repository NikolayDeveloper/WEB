using Iserv.Niis.Domain.Entities.Dictionaries;

namespace Iserv.Niis.Domain.Entities.Contract
{
    public class ContractNotificationStatus
    {
        public int ContractId { get; set; }
        public Contract Contract { get; set; }
        public int NotificationStatusId { get; set; }
        public DicNotificationStatus NotificationStatus { get; set; }
    }
}
