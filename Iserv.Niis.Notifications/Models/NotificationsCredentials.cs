using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iserv.Niis.Notifications.Models
{
    public class NotificationsCredentials
    {
        public string EmailFrom { get; set; }
        public string EmailPassword { get; set; }
        public string SmtpServer { get; set; }
        public int SmtpPort { get; set; }
        public string SmscLogin { get; set; }
        public string SmscPassword { get; set; }
    }
}
