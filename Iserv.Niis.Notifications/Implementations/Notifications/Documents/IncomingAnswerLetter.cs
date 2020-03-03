using System;
using System.Linq;
using Iserv.Niis.Notifications.Resources;
using Iserv.Niis.Common.Codes;
using Iserv.Niis.DI;
using Iserv.Niis.Domain.Entities.Document;

namespace Iserv.Niis.Notifications.Implementations.Notifications.Documents
{
    /// <summary>
    /// 001.00.ИСХ
    /// Направлено письмо. Подробнее в Личном кабинете. Хат жолданды. Толығырақ Жеке кабинетте.
    /// Napravleno pismo. Podrobnee v Lichnom kabinete. Hat zholdandy. Tolygyrak Zheke kabinette.
    /// На этапе OUT03.1 Отправка, после присвоения исходящего номера, и сохранения даты отправки.
    /// </summary>
    public class IncomingAnswerLetter : INotificationMessageRequirement<Document>
    {
        public Document CurrentRequestObject { get; set; }

        public string Message
        {
            get
            {
                return string.Format(MessageTemplates.IncomingAnswerLetter);
            }
        }

        public DateTimeOffset NotificationDate => NiisAmbientContext.Current.DateTimeProvider.Now;

        public bool IsPassesRequirements()
        {
            var docTypeCodes = new[]
            {
                DicDocumentTypeCodes.IncomingAnswerLetter,
                //DicDocumentTypeCodes.OUT_Ovt_na_Pismo_V1_19,
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