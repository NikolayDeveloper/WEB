using System.Linq;
using Iserv.Niis.Domain.Entities.Dictionaries;
using Iserv.Niis.Domain.Entities.Document;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Queries;

namespace Iserv.Niis.Notifications.Logic
{
    public class GetDocumentSmsNotificationStatusQuery : BaseQuery
    {
        public DocumentNotificationStatus Execute(int documentId)
        {
            var smsStatuses = new[]
            {
                DicNotificationStatus.Codes.PhoneNotFound, DicNotificationStatus.Codes.SmsSendFail,
                DicNotificationStatus.Codes.SmsSend, DicNotificationStatus.Codes.SmsCorrespondenceNotFound,
            };

            return Uow.GetRepository<DocumentNotificationStatus>().AsQueryable()
                .SingleOrDefault(dn => dn.DocumentId == documentId && smsStatuses.Contains(dn.NotificationStatus.Code));
        }
    }
}
