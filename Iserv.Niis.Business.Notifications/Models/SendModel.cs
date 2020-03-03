using System.Collections.Generic;

namespace Iserv.Niis.Notifications.Models
{
    public class SendModel
    {
        public IList<string> MobilePhones { get; set; }
        public IList<string> EmailAddresses { get; set; }
        public string Message { get; set; }
        public string Subject { get; set; }
        public bool IsSms { get; set; }
        public byte[] Attachment { get; set; }
        public NotificationsCredentials Credentials { get; set; }
    }
}
