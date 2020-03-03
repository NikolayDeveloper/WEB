using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Iserv.Niis.DataAccess.EntityFramework;
using Iserv.Niis.Domain.Entities.Dictionaries;
using Iserv.Niis.Domain.Entities.Document;
using Iserv.Niis.Domain.Entities.Request;
using Iserv.Niis.Domain.Entities.Security;
using Iserv.Niis.Workflow.Abstract;

namespace Iserv.Niis.Workflow.Implementations.Request.Stages.SignedDocumentApplier
{
    public class InternationalTrademarkLogic: BaseLogic
    {
        private readonly Dictionary<string, Func<Domain.Entities.Document.Document, Domain.Entities.Request.Request, Expression<Func<DicRouteStage, bool>>>> _logicMap;

        public InternationalTrademarkLogic(IWorkflowApplier<Domain.Entities.Request.Request> workflowApplier, NiisWebContext context) : base(workflowApplier, context)
        {
            _logicMap = new Dictionary<string, Func<Domain.Entities.Document.Document, Domain.Entities.Request.Request, Expression<Func<DicRouteStage, bool>>>>
            {
                {DicDocumentType.Codes.FinalNegativeExpertConclusion, DirectorApprovalLogic},
                {DicDocumentType.Codes.FinalNegativeExpertConclusionPatent, DirectorApprovalLogic},
                {DicDocumentType.Codes.FinalPositiveExpertConclusion, DirectorApprovalLogic},
            };
        }

        public override async Task ApplyAsync(ApplicationUser user, RequestDocument requestDocument)
        {
            await ApplyStageAsync(GetStagePredicate(requestDocument), requestDocument.Request);
        }

        private Expression<Func<DicRouteStage, bool>> GetStagePredicate(RequestDocument rd)
        {
            return _logicMap.ContainsKey(rd.Document.Type.Code)
                ? _logicMap[rd.Document.Type.Code].Invoke(rd.Document, rd.Request)
                : null;
        }

        private Expression<Func<DicRouteStage, bool>> DirectorApprovalLogic(Domain.Entities.Document.Document document, Domain.Entities.Request.Request request)
        {
            if (CurrentStageContains(request, "TMI03.3.9") && CurrentStageContains(document, "OUT02.3"))
            {
                return s => s.Code.Equals("TMI03.3.4");
            }

            return null;
        }
    }
}
