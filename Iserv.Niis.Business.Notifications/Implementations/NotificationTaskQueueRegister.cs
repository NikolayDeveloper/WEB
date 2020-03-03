using System;
using Iserv.Niis.DI;
using Iserv.Niis.Documents.Abstractions;
using Iserv.Niis.Domain.Helpers;
using Iserv.Niis.Notifications.Implementations.Notifications.Documents;
using Iserv.Niis.Notifications.Implementations.Notifications.ProtectionDocuments;
using Iserv.Niis.Notifications.Implementations.Notifications.Requests;
using Iserv.Niis.Notifications.Logic;
using Serilog;

namespace Iserv.Niis.Notifications.Implementations
{
    public class NotificationTaskQueueRegister
    {
        private readonly IDocumentGeneratorFactory _templateGeneratorFactory;

        public NotificationTaskQueueRegister(IDocumentGeneratorFactory templateGeneratorFactory)
        {
            _templateGeneratorFactory = templateGeneratorFactory;
        }

        public void AddNotificationsByOwnerType(int ownerId, Owner.Type ownerType, byte[] attachment = null)
        {
            switch (ownerType)
            {
                case Owner.Type.Request:
                    AddRequestNotifications(ownerId);
                    break;

                case Owner.Type.ProtectionDoc:
                    AddProtectionDocNotifications(ownerId);
                    break;

                case Owner.Type.Material:
                    AddDocumentNotificatinos(ownerId);
                    break;

                default:
                    throw new NotImplementedException();
            }
        }
        private void AddRequestNotifications(int requestId)
        {
            try
            {
                var request = NiisAmbientContext.Current.Executor.GetQuery<GetRequestByIdQuery>().Process(q => q.Execute(requestId));

                if (request == null)
                {
                    throw new Exception($"Notification register error: request with id {requestId} not found!");
                }

                var notificationTaskQueues = new RequestNotifications().GenerateNotificationTaskQueue(request);

                NiisAmbientContext.Current.Executor.GetCommand<CreateNotificationTaskQueuesCommand>().Process(c => c.Execute(notificationTaskQueues));
            }
            catch (Exception e)
            {
                Log.Error(e, "Sms notification send error: " + e.Message);
            }
        }

        private void AddDocumentNotificatinos(int documentId)
        {
            try
            {
                var document = NiisAmbientContext.Current.Executor.GetQuery<GetDocumentByIdQuery>().Process(q => q.Execute(documentId));

                if (document == null)
                {
                    throw new Exception($"Notification register error: document with id {documentId} not found!");
                }
                
                var notificationTaskQueues = new DocumentNotifications(_templateGeneratorFactory).GenerateNotificationTaskQueue(document);

                NiisAmbientContext.Current.Executor.GetCommand<CreateNotificationTaskQueuesCommand>().Process(c => c.Execute(notificationTaskQueues));
            }
            catch (Exception e)
            {
                Log.Error(e, "Sms notification send error: " + e.Message);
            }
        }

        private void AddProtectionDocNotifications(int protectionDocId)
        {
            try
            {
                var protectionDoc = NiisAmbientContext.Current.Executor.GetQuery<GetProtectionDocumentByIdQuery>().Process(q => q.Execute(protectionDocId));

                if (protectionDoc == null)
                {
                    throw new Exception($"Notification register error: protection document with id {protectionDocId} not found!");
                }

                var notificationTaskQueues = new ProtectionDocNotifications().GenerateNotificationTaskQueue(protectionDoc);

                NiisAmbientContext.Current.Executor.GetCommand<CreateNotificationTaskQueuesCommand>().Process(c => c.Execute(notificationTaskQueues));
            }
            catch (Exception e)
            {
                Log.Error(e, "Sms notification send error: " + e.Message);
            }
        }
    }
}