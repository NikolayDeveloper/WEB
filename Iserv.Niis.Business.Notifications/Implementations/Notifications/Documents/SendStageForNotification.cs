using System;
using System.Linq;
using Iserv.Niis.Notifications.Resources;
using Iserv.Niis.DI;
using Iserv.Niis.Domain.Entities.Document;
using Iserv.Niis.Notifications.Logic;
using Iserv.Niis.Common.Codes;

namespace Iserv.Niis.Notifications.Implementations.Notifications.Documents
{
    public class SendStageForNotification : INotificationMessageRequirement<Document>
    {
        public Document CurrentRequestObject { get; set; }

        public string Message
        {
            get
            {
                var requestDocument = NiisAmbientContext.Current.Executor.GetQuery<GetRequestDocumentByDocumentIdQuery>().Process(q => q.Execute(CurrentRequestObject.Id));

                return string.Format(MessageTemplates.RequestSendStageForNotification, requestDocument?.Request?.RequestNum ?? string.Empty);
            }
        }

        public DateTimeOffset NotificationDate => NiisAmbientContext.Current.DateTimeProvider.Now;

        public bool IsPassesRequirements()
        {
            //TODO: замени на коды.
            var docTypes = new[]
            {
                "VOSTPOP","VOSTPMP","VOSTIZP","UV-REG-TZ","UVP-UPPM","UVP-UPP",
                "UVP-U6","UVP-U5","UVP-U4PM","UVP-U3PM","UVP-U1PM","UVP-U1P",
                "UV-P","UV-KPM","UV-KB","UV-IZM-TZ","UV-5","UV-3PM","UV-3P_1",
                "UV-3A","UV-2PM(Z)","UV-2PM(X)","UV-2PM(O)","UV-2P(Z)","UV-2ES(O)",
                "UV-2ES","UV-2BPM","UV-2B(O)","UV-2B","UV-2(X)","UV-2(O)",
                "UV-1.2","UV-1.1","UV-1","TZ_VN_IZM_YUR_ADR_PER",
                "TZ_VN_IZM_PRED_ZAYAV","TZ_VN_IZM_PERECH_TOV","TZ_VN_IZM_NAIMEN_ZAYAV",
                "TZ_VN_IZM_ADR","TZ_VN_IZM",
                "UV_PO8","UV_PO3","UV_PO2","UV_P_PO","GR_TZ_IZMEN_NAIM",
                "ADRS","GR_TZ_IZMEN_MKTU","GR_TZ_IZMEN_ADRS",
                "GR_TZ_IZMEN","TZ_PRODL_OBSH","GR_TZ_PRODL","GR_TZ_PREKR","GR_TZ_NEDEYSTV",
                "S7","S5","S4_4","S4","S3","RKS_YVED_PM","RKS_YVED_IZ_ILG",
                "RKS_YVED_FORM","RKS_YVED","REG_2.16","REG_2.14",
                "TZREG_2.11","TZREG_2.10","REG_2.09","REG_2.08","REG_2.07",
                "REG_2.06","REG_2.05","REG_2.04","REG_2.03","REG_2.02",
                "TZREG_2.01","PRODLPOP","PRODLPMP","PRODLIZP","PREKRPM",
                "PREKRPAT_SA","PREKRPAT","PREKR_PO","TZPRED7",
                "TZPRED5","TZPRED2","NMPT1","TZPRED1.0",
                "TZPRED10","TZPRED9","TZPRED_5_1","TZPRED8","PP_IZ-IZMEN",
                "TZPOL999","TZPOL9","TZPOL8","TZPOL78","TZPOL77","TZPOL7",
                "TZPOL61_1","TZPOL5","TZPOL4","TZPOL32_UVED","GR_TZ_POL_UV",
                "PO8_1_O_P","PO8","PO3","PO2","PO1.1","PO1","PAT_PO-IZMEN",
                "PAT_PM-IZMEN_ADRESS","PAT_PM-IZMEN","PAT_IZ-IZMEN_ADRESS",
                "PAT_IZ-IZMEN","OPL_2.18","OPL_2.17","MTZ_2","MTZ_1",
                "IN_UV_EAPV","FE9","FE8","FE7","FE6","FE5","FE3",
                "FE133","FE13","FE12","FE10","FE02","FE011","FE010",
                "FE01_KZ","FE01.0","FE01","DK-U5","DK_UVED_PRIOST_DEL",
                "DK_UVED_PREK_DEL","DK_UV_CV","AP_REJECT","006.0088",
                DicDocumentTypeCodes.TZPRED1,
                DicDocumentTypeCodes.NotificationOfTmRequestReviewingAcceptance,
                DicDocumentTypeCodes.TZPRED3,
                DicDocumentTypeCodes.NotificationOfOfficeWorkTerminationByRequest,
                DicDocumentTypeCodes.NotificationOfPaymentlessOfficeWorkTermination,
                DicDocumentTypeCodes.NotificationOfAbsencePaymentOfDK,
                DicDocumentTypeCodes.OfficeWorkRestartNotification,
                DicDocumentTypeCodes.OUT_UV_Pol_Reg_TZ_AP_sov_v1_19,
                DicDocumentTypeCodes.NotificationOfRegistrationDecision,
                DicDocumentTypeCodes.ResponseTermProlongationNotification,
                DicDocumentTypeCodes.TZPOL61,
                DicDocumentTypeCodes.ResponseReleaseRequestRegistrationNotification,
                DicDocumentTypeCodes.NotificationReleaseRequestRegistrationNotification,
                DicDocumentTypeCodes.UV_TZ_VN_IZM_NAIMEN_ZAYAV,
                DicDocumentTypeCodes.UV_TZ_VN_IZM_PERECH_TOV,
                DicDocumentTypeCodes.UV_TZ_VN_IZM_PRED_ZAYAV,
                DicDocumentTypeCodes.OUT_UV_Pred_izmen_yur_adr_v1_19,
                DicDocumentTypeCodes.UV_TZ_VN_IZM_YUR_ADR_PER,
                DicDocumentTypeCodes.UV_TZ_VN_IZM,
                DicDocumentTypeCodes.TZ_USTUP_PRAVA_ZN,
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
