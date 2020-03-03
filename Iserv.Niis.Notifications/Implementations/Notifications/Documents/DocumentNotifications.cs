using System.Collections.Generic;
using Iserv.Niis.Documents.Abstractions;
using Iserv.Niis.Domain.Entities.Document;

namespace Iserv.Niis.Notifications.Implementations.Notifications.Documents
{
    public class DocumentNotifications : BaseNotification<Document>
    {
        private readonly IDocumentGeneratorFactory _templateGeneratorFactory;

        public DocumentNotifications(IDocumentGeneratorFactory templateGeneratorFactory)
        {
            _templateGeneratorFactory = templateGeneratorFactory;
        }
        internal override List<INotificationMessageRequirement<Document>> NotificationRequirements
        {
            get
            {
                return new List<INotificationMessageRequirement<Document>>
                {
                    new СreateStageForStatement(),
                    new SendStageForRequest(),
                    new SendStageForNotification(),
                    new SendStageForDecision(),
                    new RequestDocumentSendStage(_templateGeneratorFactory),
                    new CreateStageForApplication(),
                    new ContractStatementExamination(),
                    new ContractSendNotification(),
                    new ContractCreateForStatement(),
                    new RequestSendNotification(),
                    new RequestStatementExamination(),
                    new RequestStatementFullExamination(),
                    new RequestSendConclusion(),
                    new RequestSendSolution(),
                    new InvoiceForPayment(),
                    new IncomingAnswerLetter(),
                };
            }

            set { }
        }
    }
}
