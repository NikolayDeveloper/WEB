using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Iserv.Niis.DataAccess.EntityFramework;
using Iserv.Niis.Domain.Entities.Dictionaries;
using Iserv.Niis.Domain.Entities.Document;
using Iserv.Niis.Domain.Entities.Request;
using Iserv.Niis.Workflow.Abstract;

namespace Iserv.Niis.Workflow.Implementations.Request.Stages.TransferedDocumentApplier
{
    public class InternationalTrademarkLogic: BaseLogic
    {
        private readonly Dictionary<string, Func<Domain.Entities.Document.Document, Domain.Entities.Request.Request, Expression<Func<DicRouteStage, bool>>>> _logicMap;

        public InternationalTrademarkLogic(IWorkflowApplier<Domain.Entities.Request.Request> workflowApplier, NiisWebContext context) : base(workflowApplier, context)
        {
            _logicMap = new Dictionary<string, Func<Domain.Entities.Document.Document, Domain.Entities.Request.Request, Expression<Func<DicRouteStage, bool>>>>
            {
                {DicDocumentType.Codes.PreliminaryPositiveExpertConclusion, ProtectionExpertConclusionLogic},
                {DicDocumentType.Codes.FinalNegativeExpertConclusion, FinalDecisionLogic},
                {DicDocumentType.Codes.FinalNegativeExpertConclusionPatent, FinalDecisionLogic},
                {DicDocumentType.Codes.FinalPositiveExpertConclusion, FinalDecisionLogic},
                {DicDocumentType.Codes.NegativeRegistrationExpertConclusion, RefusalDecisionPublishingLogic},
                {DicDocumentType.Codes.Rejection, RefusalDecisionPublishingLogic},
                {DicDocumentType.Codes.PartialRejection, RefusalDecisionPublishingLogic},
                {DicDocumentType.Codes.PreliminaryPartialRejection, RefusalDecisionPublishingLogic},
                {DicDocumentType.Codes.PreliminaryRejection, RefusalDecisionPublishingLogic},
                {DicDocumentType.Codes.PreliminaryRejectionDisclamated, RefusalDecisionPublishingLogic},
                {DicDocumentType.Codes.ItmExpertizeRequest, RequestLogic},
                {DicDocumentType.Codes.ItmAccompanyingNoteProtection, PublishingLogic},
                {DicDocumentType.Codes.ItmAccompanyingNote, PublishingLogic},
                {DicDocumentType.Codes.Others, RefusalDecisionPublishingLogic},
                {DicDocumentType.Codes.RegistersExpertOpinionsMTI, SentToJusticeMinistryLogic},
            };
        }

        public override async Task ApplyAsync(RequestDocument requestDocument)
        {
            await ApplyStageAsync(GetStagePredicate(requestDocument), requestDocument.Request);
        }

        private Expression<Func<DicRouteStage, bool>> GetStagePredicate(RequestDocument rd)
        {
            return _logicMap.ContainsKey(rd.Document.Type.Code)
                ? _logicMap[rd.Document.Type.Code].Invoke(rd.Document, rd.Request)
                : null;
        }

        private Expression<Func<DicRouteStage, bool>> RefusalDecisionPublishingLogic(Domain.Entities.Document.Document document, Domain.Entities.Request.Request request)
        {
            if (CurrentStageContains(request, "TMI03.3.2") && CurrentStageContains(document, "OUT03.1"))
            {
                return s => s.Code.Equals("TMI03.3.3.1");
            }
            if (CurrentStageContains(request, "TMI03.3.3.1") && CurrentStageContains(document, "OUT02.1"))
            {
                return s => s.Code.Equals("TMI03.3.4.1.0");
            }

            return DirectorDecisionLogic(document, request);
        }

        private Expression<Func<DicRouteStage, bool>> RequestLogic(Domain.Entities.Document.Document document, Domain.Entities.Request.Request request)
        {
            if (CurrentStageContains(request, "TMI03.3.5") && CurrentStageContains(document, "OUT03.1"))
            {
                return s => s.Code.Equals("TMI03.3.2.0");
            }

            return null;
        }

        private Expression<Func<DicRouteStage, bool>> PublishingLogic(Domain.Entities.Document.Document document, Domain.Entities.Request.Request request)
        {
            if (CurrentStageContains(request, "TMI03.3.4.3") && CurrentStageContains(document, "OUT03.1"))
            {
                return s => s.Code.Equals("TMI03.3.4.5");
            }

            return null;
        }

        private Expression<Func<DicRouteStage, bool>> SentToJusticeMinistryLogic(Domain.Entities.Document.Document document, Domain.Entities.Request.Request request)
        {
            if (CurrentStageContains(request, "TMI03.3.4") && CurrentStageContains(document, "OUT03.1"))
            {
                return s => s.Code.Equals("TMI03.3.4.2");
            }

            return DirectorDecisionLogic(document, request);
        }

        private Expression<Func<DicRouteStage, bool>> FinalDecisionLogic(Domain.Entities.Document.Document document, Domain.Entities.Request.Request request)
        {
            if (CurrentStageContains(request, "TMI03.3.5", "TMI03.3.4.1.1") && CurrentStageContains(document, "OUT02.1"))
            {
                return s => s.Code.Equals("TMI03.3.8");
            }

            return SentToJusticeMinistryLogic(document, request);
        }

        private Expression<Func<DicRouteStage, bool>> ProtectionExpertConclusionLogic(Domain.Entities.Document.Document document, Domain.Entities.Request.Request request)
        {
            if (CurrentStageContains(request, "TMI03.3.2") && CurrentStageContains(document, "OUT02.1"))
            {
                return s => s.Code.Equals("TMI03.3.3");
            }

            return SentToJusticeMinistryLogic(document, request);
        }

        private Expression<Func<DicRouteStage, bool>> DirectorDecisionLogic(Domain.Entities.Document.Document document, Domain.Entities.Request.Request request)
        {
            if (CurrentStageContains(request, "TMI03.3.3") && CurrentStageContains(document, "OUT02.3"))
            {
                return s => s.Code.Equals("TMI03.3.4");
            }

            if (CurrentStageContains(request, "TMI03.3.8") && CurrentStageContains(document, "OUT02.3"))
            {
                return s => s.Code.Equals("TMI03.3.9");
            }

            if (CurrentStageContains(request, "TMI03.3.9") && CurrentStageContains(document, "OUT03.1")) {
                return s => s.Code.Equals("TMI03.3.4");
            }

            if (CurrentStageContains(request, "TMI03.3.3.1") && CurrentStageContains(document, "OUT03.1")) {
                return s => s.Code.Equals("TMI03.3.4");
            }

              return null;
        }
    }
}
