using System;
using System.Linq;
using Iserv.Niis.Notifications.Resources;
using Iserv.Niis.DI;
using Iserv.Niis.Domain.Entities.Document;
using Iserv.Niis.Notifications.Logic;
using Iserv.Niis.Common.Codes;

namespace Iserv.Niis.Notifications.Implementations.Notifications.Documents
{
    public class SendStageForDecision : INotificationMessageRequirement<Document>
    {
        public Document CurrentRequestObject { get; set; }

        public string Message
        {
            get
            {
                var calendarProvider = NiisAmbientContext.Current.CalendarProvider;
                var currentUserId = NiisAmbientContext.Current.User.Identity.UserId;
                var currentWf = CurrentRequestObject.CurrentWorkflows.FirstOrDefault(d => d.CurrentUserId == currentUserId || (d.DocumentWorkflowViewers != null && d.DocumentWorkflowViewers.Any(v => v.UserId == currentUserId)));

                var date = calendarProvider.GetExecutionDate(DateTimeOffset.Now, currentWf.CurrentStage.ExpirationType, currentWf.CurrentStage.ExpirationValue ?? 0);

                var requestDocument = NiisAmbientContext.Current.Executor.GetQuery<GetRequestDocumentByDocumentIdQuery>().Process(q => q.Execute(CurrentRequestObject.Id));

                return string.Format(MessageTemplates.SendStageForDecision, requestDocument?.Request?.RequestNum ?? string.Empty, date.ToString("dd/MM/yyyy"));
            }
        }

        public DateTimeOffset NotificationDate => NiisAmbientContext.Current.DateTimeProvider.Now;

        public bool IsPassesRequirements()
        {
            var docTypes = new[]
            {
                DicDocumentTypeCodes.ExpertTmRegistrationOpinionWithDisclaimer,
                DicDocumentTypeCodes.ExpertTmRegisterRefusalOpinion,
                DicDocumentTypeCodes.ExpertNmptRegisterRefusalOpinion
            };

            if (docTypes.Contains(CurrentRequestObject.Type.Code) &&
                !string.IsNullOrWhiteSpace(CurrentRequestObject.OutgoingNumber))
            {
                return true;
            }

            return false;
        }
    }
}
