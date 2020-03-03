using System.Linq;
using Iserv.Niis.Domain.Entities.Dictionaries;
using Iserv.Niis.Domain.Entities.Document;
using NetCoreCQRS.Queries;

namespace Iserv.Niis.Notifications.Logic
{
    public class GetDocumentEmailNotificationStatusQuery : BaseQuery
    {
        public DocumentNotificationStatus Execute(int documentId)
        {
            var emailStatusCodes = new[]
            {
                DicNotificationStatus.Codes.EmailNotFound, DicNotificationStatus.Codes.EmailSendFail,
                DicNotificationStatus.Codes.EmailSend, DicNotificationStatus.Codes.EmailCorrespondenceNotFound,
            };

            return Uow.GetRepository<DocumentNotificationStatus>().AsQueryable()
                .SingleOrDefault(dn => dn.DocumentId == documentId && emailStatusCodes.Contains(dn.NotificationStatus.Code));
        }
    }
}
