using System;
using System.Linq;
using Iserv.Niis.Notifications.Resources;
using Iserv.Niis.Common.Codes;
using Iserv.Niis.DI;
using Iserv.Niis.Domain.Entities.Dictionaries;
using Iserv.Niis.Domain.Entities.Document;
using Iserv.Niis.Notifications.Logic;
using Iserv.Niis.Domain.Entities.Request;

namespace Iserv.Niis.Notifications.Implementations.Notifications.Documents
{
    /// <summary>
    /// POL3111/POL0112
    /// По заявке № <Номер заявки> направлено решение. Подробнее в Личном кабинете. № <Номер заявки>  өтінім бойынша шешім жолданды. Толығырақ Жеке кабинетте.
    /// Po zajavke № <Номер заявки> napravleno reshenie. Podrobnee v Lichnom kabinete. № <Номер заявки>  otinim bojynsha sheshim zholdandy. Tolygyrak Zheke kabinette..
    /// На этапе OUT03.1 Отправка, после присвоения исходящего номера, и сохранения даты отправки.
    /// </summary>
    public class RequestSendSolution : INotificationMessageRequirement<Document>
    {
        private RequestDocument _requestDocument;
        public Document CurrentRequestObject { get; set; }

        public string Message
        {
            get
            {
                return string.Format(MessageTemplates.RequestSendSolution, _requestDocument?.Request?.RequestNum);
            }
        }

        public DateTimeOffset NotificationDate => NiisAmbientContext.Current.DateTimeProvider.Now;

        public bool IsPassesRequirements()
        {
            _requestDocument = NiisAmbientContext.Current.Executor.GetQuery<GetRequestDocumentByDocumentIdQuery>().Process(q => q.Execute(CurrentRequestObject.Id));

            if (_requestDocument == null) return false;

            var docTypeCodes = new[]
            {
                DicDocumentTypeCodes.ExpertTmRegisterRefusalOpinion,
                DicDocumentTypeCodes.ExpertTmRegisterOpinion,
                
                DicDocumentTypeCodes.OUT_Resh_pol_Chast_Reg_TZ_istech_srok_v1_19

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
