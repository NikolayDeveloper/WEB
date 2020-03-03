using System;
using System.Threading.Tasks;
using Iserv.Niis.DI;
using Iserv.Niis.Domain.Entities.ProtectionDoc;
using Iserv.Niis.Domain.Helpers;
using Iserv.Niis.Notifications.Implementations;
using Iserv.Niis.WorkflowBusinessLogic.ProtectionDocuments;
using Iserv.Niis.WorkflowBusinessLogic.WorkflowProtectionDocument;
using Iserv.Niis.WorkflowServices;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; ;
using Quartz;

namespace Iserv.Niis.Workflow.EventScheduler.Jobs
{
    public class ProtectionDocSupportJob: IJob
    {
        private readonly IExecutor _executor;

        public ProtectionDocSupportJob()
        {
            _executor = NiisAmbientContext.Current.Executor;
        }
        public async void Execute(IJobExecutionContext context)
        {
            var midnight = new DateTimeOffset(NiisAmbientContext.Current.DateTimeProvider.Now.Year,
                NiisAmbientContext.Current.DateTimeProvider.Now.Month,
                NiisAmbientContext.Current.DateTimeProvider.Now.AddDays(1).Day, 0, 0, 0,
                TimeZoneInfo.Local.GetUtcOffset(NiisAmbientContext.Current.DateTimeProvider.Now));
            var delayTime = midnight - NiisAmbientContext.Current.DateTimeProvider.Now;
            if (delayTime > TimeSpan.Zero)
            {
                await Task.Delay(delayTime);
            }

            var registerer = new NotificationTaskQueueRegister(null);

            var supportAboutToExpireProtectionDocs =
                _executor.GetQuery<GetSupportExpiredProtectionDocsQuery>().Process(q => q.Execute(1));

            foreach (var protectionDoc in supportAboutToExpireProtectionDocs)
            {
                WriteLogMessage($"Registering support about to end notification task for protection document {protectionDoc.Id}");
                registerer.AddNotificationsByOwnerType(protectionDoc.Id, Owner.Type.ProtectionDoc);
                WriteLogMessage("Registered");
            }

            var validityAboutToExpireProtectionDocs =
                _executor.GetQuery<GetValidityExpiredProtectionDocsQuery>().Process(q => q.Execute(1));

            foreach (var protectionDoc in validityAboutToExpireProtectionDocs)
            {
                WriteLogMessage($"Registering validity about to end notification task for protection document {protectionDoc.Id}");
                registerer.AddNotificationsByOwnerType(protectionDoc.Id, Owner.Type.ProtectionDoc);
                WriteLogMessage("Registered");
            }

            var supportExpiredProtectionDocs = _executor.GetQuery<GetSupportExpiredProtectionDocsQuery>().Process(q => q.Execute());

            foreach (var protectionDoc in supportExpiredProtectionDocs)
            {
                WriteLogMessage($"Registering support about to end workflow process for protection document {protectionDoc.Id}");
                ProcessProtectionDocWorkflow(protectionDoc);
                
            }

            var validityExpiredProtectionDocs = _executor.GetQuery<GetValidityExpiredProtectionDocsQuery>().Process(q => q.Execute());

            foreach (var protectionDoc in validityExpiredProtectionDocs)
            {
                WriteLogMessage($"Registering validity about to end workflow process for protection document {protectionDoc.Id}");
                ProcessProtectionDocWorkflow(protectionDoc);
            }
        }

        private void ProcessProtectionDocWorkflow(ProtectionDoc protectionDoc)
        {
            var protectionDocumentWorkFlowRequest = new ProtectionDocumentWorkFlowRequest
            {
                ProtectionDocId = protectionDoc?.Id ?? default(int),
                IsAuto = true
            };
            try
            {
                NiisWorkflowAmbientContext.Current.ProtectionDocumentWorkflowService.Process(protectionDocumentWorkFlowRequest);
            }
            catch (Exception exception)
            {
                WriteLogMessage(exception.Message);
                return;
            }            
            WriteLogMessage("Processed");
        }

        private static void WriteLogMessage(string logMessage)
        {
            Console.WriteLine(logMessage);
        }
    }
}
