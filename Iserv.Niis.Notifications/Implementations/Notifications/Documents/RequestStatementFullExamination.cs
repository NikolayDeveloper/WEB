using System;
using System.Linq;
using Iserv.Niis.Notifications.Resources;
using Iserv.Niis.Common.Codes;
using Iserv.Niis.DI;
using Iserv.Niis.Domain.Entities.Document;
using Iserv.Niis.Notifications.Logic;
using Iserv.Niis.Domain.Entities.Request;
using Iserv.Niis.Business.Abstract;
namespace Iserv.Niis.Notifications.Implementations.Notifications.Documents
{
    /// <summary>
    /// По заявке № <Номер заявки> направлен запрос. Подробнее в Личном кабинете. № <Номер заявки> өтінім бойынша сұрату жолданды. Толығырақ Жеке кабинетте.
    /// Po zajavke № <Номер заявки> napravlen zapros. Podrobnee v Lichnom kabinete. № <Номер заявки> otinim bojynsha suratu zholdandy. 
    /// На этапе OUT03.1 Отправка, после присвоения исходящего номера, и сохранения даты отправки.
    /// </summary>
    class RequestStatementFullExamination : INotificationMessageRequirement<Document>
    {
        private RequestDocument _requestDocument;
        public Document CurrentRequestObject { get; set; }

        private readonly ICalendarProvider _calendarProvider;
        
        public string Message
        {
            get
            {
                return string.Format(MessageTemplates.RequestStatementFullExamination, _requestDocument?.Request?.RequestNum);
            }
        }

        public DateTimeOffset NotificationDate => NiisAmbientContext.Current.DateTimeProvider.Now;

        public bool IsPassesRequirements()
        {
            _requestDocument = NiisAmbientContext.Current.Executor.GetQuery<GetRequestDocumentByDocumentIdQuery>().Process(q => q.Execute(CurrentRequestObject.Id));

            if (_requestDocument == null) return false;

            if (_requestDocument.Request.CurrentWorkflow.CurrentStage.Code != RouteStageCodes.TZFirstFullExpertize)
                return false;

            var docTypeCodes = new[]
            {
                //DicDocumentTypeCodes.RequestOfprocuratory,
                //DicDocumentTypeCodes.RequestForImagesTA,
                //DicDocumentTypeCodes.OUT_Zap_Pred_Prior_v1_19,
                //DicDocumentTypeCodes.OUT_Zap_Pred_Perevod_v1_19,
                //DicDocumentTypeCodes.OUT_Zap_Pred_Nesootv_MKTU_v1_19,
                //DicDocumentTypeCodes.OUT_Zap_Pred_Adres_v1_19,
                //DicDocumentTypeCodes.OUT_Zap_Pred_ECO_v1_19,
                //DicDocumentTypeCodes.OUT_Zap_Pred_otsuts_MKTU_v1_19,
                //DicDocumentTypeCodes.OUT_Zap_Pred_perevod_oboz_v1_19,
                //DicDocumentTypeCodes.OUT_Zap_Pol_perevod_oboz_v1_19,
                DicDocumentTypeCodes.OUT_Zap_Pol_pismo_sogl_v1_19,
                //DicDocumentTypeCodes.OUT_Zap_Pred_kach_izobr_v1_19,
                //DicDocumentTypeCodes.OUT_Zap_Pol_kach_izobr_v1_19,
                //DicDocumentTypeCodes.OUT_Zap_Pred_otsut_izobr_v1_19,
                DicDocumentTypeCodes.OUT_Zap_Pred_gos_sim_v1_19,
                DicDocumentTypeCodes.OUT_Zap_Pol_gos_sim_v1_19,
                //DicDocumentTypeCodes.OUT_Zap_Pred_dover_v1_19,
                //DicDocumentTypeCodes.OUT_Zap_Pol_dover_v1_19,
                DicDocumentTypeCodes.OUT_Zap_Pol_avtor_v1_19,
                DicDocumentTypeCodes.OUT_Zap_Pol_lich_neimush_v1_19,
                DicDocumentTypeCodes.OUT_Zap_Pol_obyect_sv_v1_19,
                DicDocumentTypeCodes.OUT_Zap_Pol_kult_dost_v1_19
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
