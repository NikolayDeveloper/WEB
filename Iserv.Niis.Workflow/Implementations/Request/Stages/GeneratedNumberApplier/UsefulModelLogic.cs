using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Iserv.Niis.DataAccess.EntityFramework;
using Iserv.Niis.Domain.Entities.Dictionaries;
using Iserv.Niis.Domain.Entities.Request;
using Iserv.Niis.Workflow.Abstract;

namespace Iserv.Niis.Workflow.Implementations.Request.Stages.GeneratedNumberApplier
{
    public class UsefulModelLogic : BaseLogic
    {
        private readonly Dictionary<string, Func<Domain.Entities.Request.Request, Expression<Func<DicRouteStage, bool>>>> _logicMap;

        public UsefulModelLogic(IWorkflowApplier<Domain.Entities.Request.Request> workflowApplier, NiisWebContext context) : base(workflowApplier, context)
        {
            _logicMap = new Dictionary<string, Func<Domain.Entities.Request.Request, Expression<Func<DicRouteStage, bool>>>>
            {
                {DicDocumentType.Codes.UV_KPM, UV_KPM},
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

        private Expression<Func<DicRouteStage, bool>> UV_KPM(Domain.Entities.Request.Request request)
        {
            if (CurrentStageContains(request, "U03.9") && AnyDocuments(request, DicDocumentType.Codes.DecisionOfAppealsBoard))
            {
                return s => s.Code.Equals("U03.7.1");
            }

            return null;
        }
    }
}
