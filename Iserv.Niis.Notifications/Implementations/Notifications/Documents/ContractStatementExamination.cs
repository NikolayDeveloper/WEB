using System;
using System.Linq;
using Iserv.Niis.Notifications.Resources;
using Iserv.Niis.Common.Codes;
using Iserv.Niis.DI;
using Iserv.Niis.Domain.Entities.Contract;
using Iserv.Niis.Domain.Entities.Dictionaries;
using Iserv.Niis.Domain.Entities.Document;
using Iserv.Niis.Notifications.Logic;

namespace Iserv.Niis.Notifications.Implementations.Notifications.Documents
{
    public class ContractStatementExamination : INotificationMessageRequirement<Document>
    {
        private ContractDocument _contractDocument;
        public Document CurrentRequestObject { get; set; }

        public string Message
        {
            get
            {
                var contractNum = _contractDocument.Contract.ContractNum;
                var onlineStatusName = _contractDocument.Contract.CurrentWorkflow.CurrentStage?.OnlineRequisitionStatus?.NameRu;

                return string.Format(MessageTemplates.ContractStatementExamination, contractNum ?? string.Empty, onlineStatusName);
            }
        }

        public DateTimeOffset NotificationDate => NiisAmbientContext.Current.DateTimeProvider.Now;

        public bool IsPassesRequirements()
        {
            _contractDocument = NiisAmbientContext.Current.Executor.GetQuery<GetContractDocumentByDocumentIdQuery>().Process(q => q.Execute(CurrentRequestObject.Id));

            if (_contractDocument == null) return false;

            var routeStageCodes = new[]
            {
                RouteStageCodes.PO_03_2_1,
                RouteStageCodes.I_03_3_1_1,
                RouteStageCodes.SA_03_2_1,
                RouteStageCodes.UM_03_2_1,
            };

            if (CurrentRequestObject.Type.Code.Equals(DicDocumentTypeCodes.Statement)
                && CurrentRequestObject.CurrentWorkflows.Any(cwf => cwf.CurrentStage.Code.Equals(DicRouteStage.Codes.SentStage))
                && routeStageCodes.Contains(_contractDocument.Contract.CurrentWorkflow.CurrentStage.Code))
            {
                return true;
            }

            return false;
        }
    }
}
