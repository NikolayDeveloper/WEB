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
    public class UsefulModelLogic : BaseLogic
    {
        private readonly Dictionary<string, Func<Domain.Entities.Request.Request, Expression<Func<DicRouteStage, bool>>>> _logicMap;

        public UsefulModelLogic(IWorkflowApplier<Domain.Entities.Request.Request> workflowApplier, NiisWebContext context) : base(workflowApplier, context)
        {
            _logicMap = new Dictionary<string, Func<Domain.Entities.Request.Request, Expression<Func<DicRouteStage, bool>>>>
            {
                {DicDocumentType.Codes.AnswerToRequest, AnswerToRequest},
				{DicDocumentType.Codes.PetitionForRestoreTime, AnswerToRequest},
				{DicDocumentType.Codes.DecisionOfAuthorizedBody, DecisionOfAuthorizedBody},
                {DicDocumentType.Codes.Objection,  Objection},

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

        private Expression<Func<DicRouteStage, bool>> AnswerToRequest(Domain.Entities.Request.Request request)
        {
            if (CurrentStageContains(request, "U02.2.6") && AnyDocuments(request, DicDocumentType.Codes.AnswerToRequest))
            {
                return s => s.Code.Equals("U03.2");
			}
			if (CurrentStageContains(request, "U03.2.1") && AnyDocuments(request, DicDocumentType.Codes.AnswerToRequest))
			{
				return s => s.Code.Equals("U03.2");
			}
			if (CurrentStageContains(request, "U03.3.1") && AnyDocuments(request, DicDocumentType.Codes.PetitionForRestoreTime) && AnyDocuments(request, DicDocumentType.Codes.AnswerToRequest) && HasPaidInvoices(request, "NEW_166"))
			{
				return s => s.Code.Equals("U03.2");
			}

			return null;
		}

		private Expression<Func<DicRouteStage, bool>> DecisionOfAuthorizedBody(Domain.Entities.Request.Request request)
		{
			if (CurrentStageContains(request, "U03.5"))
			{
				return s => s.Code.Equals("U03.6");
			}
			return null;
		}

        private Expression<Func<DicRouteStage, bool>> Objection(Domain.Entities.Request.Request request) {
            if (CurrentStageContains(request, "U03.4.0")) {
                return s => s.Code.Equals("U03.9");
            }
            return null;
		}

	}
}
