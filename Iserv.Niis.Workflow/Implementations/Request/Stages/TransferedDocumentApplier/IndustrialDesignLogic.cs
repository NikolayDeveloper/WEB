using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Iserv.Niis.DataAccess.EntityFramework;
using Iserv.Niis.Domain.Entities.Dictionaries;
using Iserv.Niis.Domain.Entities.Request;
using Iserv.Niis.Workflow.Abstract;

namespace Iserv.Niis.Workflow.Implementations.Request.Stages.TransferedDocumentApplier {
    public class IndustrialDesignLogic : BaseLogic {
        private readonly Dictionary<string, Func<Domain.Entities.Document.Document, Domain.Entities.Request.Request, Expression<Func<DicRouteStage, bool>>>> _logicMap;

        public IndustrialDesignLogic(IWorkflowApplier<Domain.Entities.Request.Request> workflowApplier, NiisWebContext context) : base(workflowApplier, context) {
            _logicMap = new Dictionary<string, Func<Domain.Entities.Document.Document, Domain.Entities.Request.Request, Expression<Func<DicRouteStage, bool>>>> {

            };
        }

        public override async Task ApplyAsync(RequestDocument requestDocument) {
            await ApplyStageAsync(GetStagePredicate(requestDocument), requestDocument.Request);
        }

        private Expression<Func<DicRouteStage, bool>> GetStagePredicate(RequestDocument rd) {
            return _logicMap.ContainsKey(rd.Document.Type.Code)
                ? _logicMap[rd.Document.Type.Code].Invoke(rd.Document, rd.Request)
                : null;
        }
    }
}
