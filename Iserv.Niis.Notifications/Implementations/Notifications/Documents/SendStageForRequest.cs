using System;
using Iserv.Niis.Notifications.Resources;
using Iserv.Niis.Domain.Entities.Document;
using System.Linq;
using Iserv.Niis.Notifications.Logic;
using Iserv.Niis.DI;
using Iserv.Niis.Common.Codes;

namespace Iserv.Niis.Notifications.Implementations.Notifications.Documents
{
    public class SendStageForRequest : INotificationMessageRequirement<Document>
    {
        public Document CurrentRequestObject { get; set; }

        public string Message
        {
            get
            {
                var calendarProvider = NiisAmbientContext.Current.CalendarProvider;

                var requestDocument = NiisAmbientContext.Current.Executor.GetQuery<GetRequestDocumentByDocumentIdQuery>().Process(q => q.Execute(CurrentRequestObject.Id));

                var date = calendarProvider.GetExecutionDate(DateTimeOffset.Now, requestDocument.Request.CurrentWorkflow.CurrentStage.ExpirationType, 
                    (short)requestDocument.Request.CurrentWorkflow.CurrentStage.ExpirationValue);
                return string.Format(MessageTemplates.SendStageForRequest, requestDocument?.Request?.RequestNum ?? string.Empty, date.ToString("dd/MM/yyyy"));
            }
        }

        public DateTimeOffset NotificationDate => NiisAmbientContext.Current.DateTimeProvider.Now;

        public bool IsPassesRequirements()
        {
            var docTypes = new[]
            {
                "S1","S1","PO7","TZPRED4","IZ-2A-KZ","IZ-2B",
                "PM2","TZPOL10","TZ-ZAPROS_GR","FE11",
                "TZ_ZAP_O_DOV","TZ_ZAP_O_IZOBR","Z_PO7",
                "IZ-2B_FILIAL","DK_ZAPROS","MTZ_3","PO7",
                "IZ-2A-KZ","IZ-2B","PM2",
                "TZ-ZAPROS_GR","FE11","TZ_ZAP_O_DOV",
                "TZ_ZAP_O_IZOBR","Z_PO7","IZ-2B_FILIAL",
                "DK_ZAPROS","MTZ_3",
                //DicDocumentTypeCodes.OUT_Zap_Pred_otsuts_MKTU_v1_19,
                //DicDocumentTypeCodes.OUT_Zap_Pol_dover_v1_19
            };

            if (docTypes.Contains(CurrentRequestObject.Type.Code)
                && !string.IsNullOrEmpty(CurrentRequestObject.OutgoingNumber))
            {
                return true;
            }

            return false;
        }
    }
}
