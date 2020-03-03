using System.Linq;
using System.Threading.Tasks;
using Iserv.Niis.DataAccess.EntityFramework;
using Iserv.Niis.Domain.Entities.Dictionaries;
using Iserv.Niis.Domain.Entities.Document;
using Iserv.Niis.Domain.Entities.Request;

namespace Iserv.Niis.Notifications
{
    /// <summary>
    /// ЧТО КЛАСС ДЕЛАЕТ из названия отправляет, на деле работает с базой 
    /// </summary>
    public abstract class NotificationSenderBase
    {
        private readonly NiisWebContext _context;
        protected (int id, string statusCode) Model; 

        protected NotificationSenderBase(NiisWebContext context)
        {
            _context = context;
        }

        protected async Task UpdateSmsDocuments()
        {
            var status = _context.DicNotificationStatuses.Single(ns => ns.Code == Model.statusCode);
            var documentStatus = _context.DocumentNotificationStatuses
                .SingleOrDefault(dn => dn.DocumentId.Equals(Model.id) && new[]
                {
                    DicNotificationStatus.Codes.PhoneNotFound, DicNotificationStatus.Codes.SmsSendFail,
                    DicNotificationStatus.Codes.SmsSend, DicNotificationStatus.Codes.SmsCorrespondenceNotFound,
                }.Contains(dn.NotificationStatus.Code));

            if (documentStatus != null)
            {
                _context.DocumentNotificationStatuses.Remove(documentStatus);
                await _context.SaveChangesAsync();
            }

            _context.DocumentNotificationStatuses.Add(
                new DocumentNotificationStatus
                {
                    DocumentId = Model.id,
                    NotificationStatusId = status.Id
                });
            await _context.SaveChangesAsync();
        }

        protected async Task UpdateEmailDocumentsAsync()
        {
            var status = _context.DicNotificationStatuses.Single(ns =>
                ns.Code == Model.statusCode);

            var documentStatus = _context.DocumentNotificationStatuses
                .SingleOrDefault(dn => dn.DocumentId.Equals(Model.id) && new[]
                {
                    DicNotificationStatus.Codes.EmailNotFound, DicNotificationStatus.Codes.EmailSendFail,
                    DicNotificationStatus.Codes.EmailSend, DicNotificationStatus.Codes.EmailCorrespondenceNotFound,
                }.Contains(dn.NotificationStatus.Code));

            if (documentStatus != null)
            {
                _context.DocumentNotificationStatuses.Remove(documentStatus);
                await _context.SaveChangesAsync();
            }

            _context.DocumentNotificationStatuses.Add(
                new DocumentNotificationStatus
                {
                    DocumentId = Model.id,
                    NotificationStatusId = status.Id
                });

            await _context.SaveChangesAsync();
        }

        protected async Task UpdateReqeuestsAsync()
        {
            var status = _context.DicNotificationStatuses.Single(ns =>
                ns.Code == Model.statusCode);

            var requestStatus = _context.RequestNotificationStatuses
                .SingleOrDefault(rn => rn.RequestId.Equals(Model.id));

            if (requestStatus != null)
            {
                _context.RequestNotificationStatuses.Remove(requestStatus);
                await _context.SaveChangesAsync();
            }

            _context.RequestNotificationStatuses.Add(
                new RequestNotificationStatus
                {
                    RequestId = Model.id,
                    NotificationStatusId = status.Id
                });
            _context.SaveChangesAsync().Wait();
        }
    }
}
