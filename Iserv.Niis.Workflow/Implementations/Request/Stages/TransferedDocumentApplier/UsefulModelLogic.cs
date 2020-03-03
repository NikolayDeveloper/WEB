using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Iserv.Niis.DataAccess.EntityFramework;
using Iserv.Niis.Domain.Entities.Dictionaries;
using Iserv.Niis.Domain.Entities.Request;
using Iserv.Niis.Workflow.Abstract;

namespace Iserv.Niis.Workflow.Implementations.Request.Stages.TransferedDocumentApplier
{
    public class UsefulModelLogic : BaseLogic
    {
        private readonly Dictionary<string, Func<Domain.Entities.Document.Document, Domain.Entities.Request.Request, Expression<Func<DicRouteStage, bool>>>> _logicMap;

        public UsefulModelLogic(IWorkflowApplier<Domain.Entities.Request.Request> workflowApplier, NiisWebContext context) : base(workflowApplier, context)
        {
            _logicMap = new Dictionary<string, Func<Domain.Entities.Document.Document, Domain.Entities.Request.Request, Expression<Func<DicRouteStage, bool>>>>
            {
				 {DicDocumentType.Codes.PaymentNotification, PaymentNotification},
				 {DicDocumentType.Codes.NotPaymentRequest, NotPaymentRequest},
				 {DicDocumentType.Codes.GrantingPatentConclusion, ExpertConclusion},
				 {DicDocumentType.Codes.RefusalToGratConclusion, ExpertConclusion},
				 {DicDocumentType.Codes.GrantingUsefulModel, ExpertConclusion},
				 {DicDocumentType.Codes.ExpertizeRequest, ExpertizeRequest},
				 {DicDocumentType.Codes.FormPM2, ExpertizeRequest},
				 {DicDocumentType.Codes.UV_2PMZ, PeriodRecovery},
				 {DicDocumentType.Codes.ReestrExpertConclusionToMJ, TransferredToMJ},
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
				? _logicMap[rd.Document.Type.Code].Invoke(rd.Document, rd.Request)
				: null;
		}

		private Expression<Func<DicRouteStage, bool>> PaymentNotification(Domain.Entities.Document.Document document, Domain.Entities.Request.Request request)
		{
			if (CurrentStageContains(request, "U02.2") && CurrentStageContains(document, "OUT03.1"))
			{
				return s => s.Code.Equals("U02.2.7");
			}

			return null;
		}

		private Expression<Func<DicRouteStage, bool>> NotPaymentRequest(Domain.Entities.Document.Document document, Domain.Entities.Request.Request request)
		{
			if (CurrentStageContains(request, "X01") && CurrentStageContains(document, "OUT03.1"))
			{
				return s => s.Code.Equals("U02.2.0");
			}
			return null;
		}

		private Expression<Func<DicRouteStage, bool>> ExpertizeRequest(Domain.Entities.Document.Document document, Domain.Entities.Request.Request request)
		{
			if (CurrentStageContains(request, "U03.2") && CurrentStageContains(document, "OUT03.1"))
			{
				return s => s.Code.Equals("U03.2.1");
			}
			return null;
		}

		private Expression<Func<DicRouteStage, bool>> ExpertConclusion(Domain.Entities.Document.Document document, Domain.Entities.Request.Request request)
		{
			if (CurrentStageContains(request, "U03.2") && CurrentStageContains(document, "OUT02.1"))
			{
				return s => s.Code.Equals("U03.3");
			}
			if (CurrentStageContains(request, "U03.3") && CurrentStageContains(document, "OUT03.1"))
			{
				return s => s.Code.Equals("U03.4");
			}
			return null;
		}

		private Expression<Func<DicRouteStage, bool>> PeriodRecovery(Domain.Entities.Document.Document document, Domain.Entities.Request.Request request)
		{
			if (CurrentStageContains(request, "X01") && CurrentStageContains(document, "OUT03.1"))
			{
				return s => s.Code.Equals("U03.3.1");
			}
			return null;
		}

		private Expression<Func<DicRouteStage, bool>> TransferredToMJ(Domain.Entities.Document.Document document, Domain.Entities.Request.Request request)
		{
			if (CurrentStageContains(request, "U03.4") && CurrentStageContains(document, "OUT03.1"))
			{
				return s => s.Code.Equals("U03.5");
			}
			return null;
		}

		private Expression<Func<DicRouteStage, bool>> UV_KPM(Domain.Entities.Document.Document document, Domain.Entities.Request.Request request)
		{
			if (CurrentStageContains(request, "U03.6") && CurrentStageContains(document, "OUT03.1"))
			{
				return s => s.Code.Equals("U03.7.1");
			}
			return null;
		}


		


	}
}
