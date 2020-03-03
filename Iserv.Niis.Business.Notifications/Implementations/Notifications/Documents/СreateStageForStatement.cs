using System;
using System.Linq;
using Iserv.Niis.Notifications.Resources;
using Iserv.Niis.Common.Codes;
using Iserv.Niis.DI;
using Iserv.Niis.Domain.Entities.Dictionaries;
using Iserv.Niis.Domain.Entities.Document;
using Iserv.Niis.Domain.Entities.Request;
using Iserv.Niis.Notifications.Logic;

namespace Iserv.Niis.Notifications.Implementations.Notifications.Documents
{
    public class СreateStageForStatement : INotificationMessageRequirement<Document>
    {
        private RequestDocument _requestDocument;
        public Document CurrentRequestObject { get; set; }

        public string Message
        {
            get
            {
                var calendarProvider = NiisAmbientContext.Current.CalendarProvider;
                var currentUserId = NiisAmbientContext.Current.User.Identity.UserId;
                var currentWf = CurrentRequestObject.CurrentWorkflows.FirstOrDefault(d => d.CurrentUserId == currentUserId || (d.DocumentWorkflowViewers != null && d.DocumentWorkflowViewers.Any(v => v.UserId == currentUserId)));

                var date = calendarProvider.GetExecutionDate(DateTimeOffset.Now, currentWf.CurrentStage.ExpirationType, (short)currentWf.CurrentStage.ExpirationValue);

                return string.Format(MessageTemplates.SendStageForStatement, _requestDocument.Request.RequestNum ?? string.Empty);
            }
        }

        public DateTimeOffset NotificationDate => NiisAmbientContext.Current.DateTimeProvider.Now;

        public bool IsPassesRequirements()
        {
            _requestDocument = NiisAmbientContext.Current.Executor.GetQuery<GetRequestDocumentByDocumentIdQuery>().Process(q => q.Execute(CurrentRequestObject.Id));

            if (_requestDocument == null) return false;

            var routeStageCodes = new[]
            {
                RouteStageCodes.PO_02_1,
                RouteStageCodes.I_02_1,
                RouteStageCodes.NMPT_02_1,
                RouteStageCodes.SA_02_1,
                RouteStageCodes.ITZ_02_1,
                RouteStageCodes.TZ_02_1,
                RouteStageCodes.UM_02_1,
            };

            if (CurrentRequestObject.Type.Code.Equals(DicDocumentTypeCodes.Statement)
                && CurrentRequestObject.CurrentWorkflows.Any(cwf => cwf.CurrentStage.Code.Equals(DicRouteStage.Codes.SentStage))
                && routeStageCodes.Contains(_requestDocument.Request.CurrentWorkflow.CurrentStage.Code))
            {
                return true;
            }

            return false;
        }        
    }
}
