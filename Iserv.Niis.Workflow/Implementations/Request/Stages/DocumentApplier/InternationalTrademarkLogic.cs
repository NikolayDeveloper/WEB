using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Iserv.Niis.DataAccess.EntityFramework;
using Iserv.Niis.Domain.Entities.Dictionaries;
using Iserv.Niis.Domain.Entities.Request;
using Iserv.Niis.Workflow.Abstract;

namespace Iserv.Niis.Workflow.Implementations.Request.Stages.DocumentApplier
{
    public class InternationalTrademarkLogic: BaseLogic
    {
        private readonly Dictionary<string, Func<Domain.Entities.Request.Request, Expression<Func<DicRouteStage, bool>>>> _logicMap;

        public InternationalTrademarkLogic(IWorkflowApplier<Domain.Entities.Request.Request> workflowApplier, NiisWebContext context) : base(workflowApplier, context)
        {
            _logicMap = new Dictionary<string, Func<Domain.Entities.Request.Request, Expression<Func<DicRouteStage, bool>>>>
            {
                {DicDocumentType.Codes.Objection, PreliminaryOrFinalRejectionObjectionLogic},
                {DicDocumentType.Codes.DecisionOfAuthorizedBody, JusticeMinistryReplyLogic},
                {DicDocumentType.Codes.AnswerToRequest, ObjectionLogic},
            };
        }

        public override async Task ApplyAsync(RequestDocument requestDocument)
        {
            await ApplyStageAsync(GetStagePredicate(requestDocument), requestDocument.Request);
        }

        private Expression<Func<DicRouteStage, bool>> GetStagePredicate(RequestDocument rd)
        {
            return _logicMap.ContainsKey(rd.Document.Type.Code)
                ? _logicMap[rd.Document.Type.Code].Invoke(rd.Request)
                : null;
        }

        private Expression<Func<DicRouteStage, bool>> PreliminaryOrFinalRejectionObjectionLogic(Domain.Entities.Request.Request request)
        {
            if (CurrentStageContains(request, "TMI03.3.4.1.0", "TMI03.3.4.5.1"))
            {
                return s => s.Code.Equals("TMI03.3.5");
            }

            return null;
        }

        private Expression<Func<DicRouteStage, bool>> JusticeMinistryReplyLogic(Domain.Entities.Request.Request request)
        {
            if (CurrentStageContains(request, "TMI03.3.4.2"))
            {
                return s => s.Code.Equals("TMI03.3.4.3");
            }

            return null;
        }

        private Expression<Func<DicRouteStage, bool>> ObjectionLogic(Domain.Entities.Request.Request request)
        {
            if ((CurrentStageContains(request, "TMI03.3.4.5.1") && FromStageContains(request, "TMI03.3.2.0"))
                || (CurrentStageContains(request, "TMI03.3.2.0")))
            {
                return s => s.Code.Equals("TMI03.3.5");
            }

            return null;
        }
    }
}
