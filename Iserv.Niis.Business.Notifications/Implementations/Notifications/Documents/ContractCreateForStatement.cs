using System;
using Iserv.Niis.Notifications.Resources;
using Iserv.Niis.Common.Codes;
using Iserv.Niis.DI;
using Iserv.Niis.Domain.Entities.Contract;
using Iserv.Niis.Domain.Entities.Dictionaries;
using Iserv.Niis.Domain.Entities.Document;
using Iserv.Niis.Notifications.Logic;

namespace Iserv.Niis.Notifications.Implementations.Notifications.Documents
{
    public class ContractCreateForStatement : INotificationMessageRequirement<Document>
    {
        private ContractDocument _contractDocument;
        public Document CurrentRequestObject { get; set; }

        public string Message
        {
            get
            {
                return string.Format(MessageTemplates.ContractCreateForStatement, _contractDocument?.Contract?.ContractNum ?? string.Empty);
            }
        }

        public DateTimeOffset NotificationDate => NiisAmbientContext.Current.DateTimeProvider.Now;

        public bool IsPassesRequirements()
        {
            _contractDocument = NiisAmbientContext.Current.Executor.GetQuery<GetContractDocumentByDocumentIdQuery>().Process(q => q.Execute(CurrentRequestObject.Id));

            if (_contractDocument == null) return false;

            if (CurrentRequestObject.Type.Code == DicDocumentTypeCodes.Statement
                && _contractDocument.Contract.CurrentWorkflow.CurrentStage.Code == RouteStageCodes.DK02_1)
            {
                return true;
            }

            return false;
        }
    }
}
