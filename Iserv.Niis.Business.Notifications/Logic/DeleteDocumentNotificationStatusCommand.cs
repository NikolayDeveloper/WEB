﻿
using Iserv.Niis.Domain.Entities.Document;
using NetCoreCQRS.Commands;

namespace Iserv.Niis.Notifications.Logic
{
    public class DeleteDocumentNotificationStatusCommand : BaseCommand
    {
        public void Execute(DocumentNotificationStatus documentNotificationStatus)
        {
            Uow.GetRepository<DocumentNotificationStatus>().Delete(documentNotificationStatus);
            Uow.SaveChanges();
        }
    }
}
