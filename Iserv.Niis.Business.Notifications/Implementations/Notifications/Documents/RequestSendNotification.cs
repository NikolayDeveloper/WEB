using System;
using System.Linq;
using Iserv.Niis.Notifications.Resources;
using Iserv.Niis.Common.Codes;
using Iserv.Niis.DI;
using Iserv.Niis.Domain.Entities.Document;
using Iserv.Niis.Notifications.Logic;
using Iserv.Niis.Domain.Entities.Request;

namespace Iserv.Niis.Notifications.Implementations.Notifications.Documents
{
    /// <summary>
    /// По заявке № <Номер заявки> направлено уведомление. Подробнее в Личном кабинете. № <Номер заявки>  өтінім бойынша хабарлама жолданды. Толығырақ Жеке кабинетте.
    /// Po zajavke № <Номер заявки> napravleno uvedomlenie. Podrobnee v Lichnom kabinete. № <Номер заявки>  otinim bojynsha habarlama zholdandy. Tolygyrak Zheke kabinette.
    /// На этапе OUT03.1 Отправка, после присвоения исходящего номера, и сохранения даты отправки.
    /// </summary>
    public class RequestSendNotification : INotificationMessageRequirement<Document>
    {
        private RequestDocument _requestDocument;
        public Document CurrentRequestObject { get; set; }

        public string Message
        {
            get
            {
                return string.Format(MessageTemplates.RequestSendNotification, _requestDocument?.Request?.RequestNum);
            }
        }

        public DateTimeOffset NotificationDate => NiisAmbientContext.Current.DateTimeProvider.Now;

        public bool IsPassesRequirements()
        {
            _requestDocument = NiisAmbientContext.Current.Executor.GetQuery<GetRequestDocumentByDocumentIdQuery>().Process(q => q.Execute(CurrentRequestObject.Id));

            if (_requestDocument == null) return false;

            var docTypeCodes = new[]
            {
                DicDocumentTypeCodes.NotificationOfAnswerlessOfficeWorkTermination,
                DicDocumentTypeCodes.CoveringLetterOfDkAboutDelay,
                DicDocumentTypeCodes.CoveringLetterOfDkAboutDelay_1,
                DicDocumentTypeCodes.NotificationOfTrademarkRequestRegistation,
                DicDocumentTypeCodes.NotificationOfTmRegistration,
                DicDocumentTypeCodes.ResponseDelapFormsExp,
                DicDocumentTypeCodes.RequestForPreExamination,
                DicDocumentTypeCodes.RequestForFullExamination,
                

                DicDocumentTypeCodes.ExpertTmRegistrationOpinionWithDisclaimer,
                DicDocumentTypeCodes.ExpertTmRegisterOpinion,
                DicDocumentTypeCodes.TrademarkRegistrationDecision,
                DicDocumentTypeCodes.TZPRED1,
                DicDocumentTypeCodes.NotificationOfTmRequestReviewingAcceptance,
                DicDocumentTypeCodes.TZPRED3,
                DicDocumentTypeCodes.NotificationOfTrademarkRequestRegistationByRequest,
                DicDocumentTypeCodes.NotificationOfOfficeWorkTerminationByRequest,
                DicDocumentTypeCodes.NotificationOfPaymentlessOfficeWorkTermination,
                DicDocumentTypeCodes.NotificationOfAbsencePaymentOfDK,
                DicDocumentTypeCodes.ResponseDelapFormsExp,
                DicDocumentTypeCodes.OfficeWorkRestartNotification,
                DicDocumentTypeCodes.OUT_UV_Pol_Reg_TZ_AP_sov_v1_19,
                DicDocumentTypeCodes.UV_KPM,
                DicDocumentTypeCodes.NotificationOfRegistrationDecision,
                DicDocumentTypeCodes.OUT_UV_Pred_Prekr_del_bez_opl_v1_19,
                DicDocumentTypeCodes.ResponseTermProlongationNotification,
                DicDocumentTypeCodes.MTZ_NEW_7,
                DicDocumentTypeCodes.MTZ_NEW_6,
                DicDocumentTypeCodes.TZPOL61,
                DicDocumentTypeCodes.OUT_UV_Pred_preobr_zayav_TZ_na_KTZ_v1_19,
                DicDocumentTypeCodes.ResponseReleaseRequestRegistrationNotification,
                DicDocumentTypeCodes.NotificationReleaseRequestRegistrationNotification,
                DicDocumentTypeCodes.OUT_UV_Pred_izmen_adres_v1_19,
                DicDocumentTypeCodes.UV_TZ_VN_IZM_NAIMEN_ZAYAV,
                DicDocumentTypeCodes.OUT_UV_Pred_izmen_naim_adr_v1_19,
                DicDocumentTypeCodes.UV_TZ_VN_IZM_PERECH_TOV,
                DicDocumentTypeCodes.UV_TZ_VN_IZM_PRED_ZAYAV,
                DicDocumentTypeCodes.OUT_UV_Pred_izmen_yur_adr_v1_19,
                DicDocumentTypeCodes.UV_TZ_VN_IZM_YUR_ADR_PER,
                DicDocumentTypeCodes.UV_TZ_VN_IZM,
                DicDocumentTypeCodes.OUT_UV_Pol_neprin_hod_v1_19,
                DicDocumentTypeCodes.TZ_USTUP_PRAVA_ZN,
                DicDocumentTypeCodes.TZ_USTUP_PRAVA_ZN,
                DicDocumentTypeCodes.OUT_UV_GR_neprin_zayav_v1_19,

            };

            if (CurrentRequestObject.CurrentWorkflows.Any(cwf => cwf.CurrentStage.Code == RouteStageCodes.DocumentOutgoing_03_1)
                && !string.IsNullOrEmpty(CurrentRequestObject.OutgoingNumber)
                && docTypeCodes.Contains(CurrentRequestObject.Type.Code)
                )
            {
                return true;
            }

            return false;
        }
    }
}
