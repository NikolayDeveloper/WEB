using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Iserv.Niis.DataAccess.EntityFramework;
using Iserv.Niis.Domain.Entities.Dictionaries;
using Iserv.Niis.Domain.Entities.Request;
using Iserv.Niis.Domain.Entities.Security;
using Iserv.Niis.Workflow.Abstract;

namespace Iserv.Niis.Workflow.Implementations.Request.Stages.SignedDocumentApplier
{
    public class TrademarkLogic : BaseLogic
    {
        private readonly Dictionary<string, Func<Domain.Entities.Request.Request, Expression<Func<DicRouteStage, bool>>>> _logicMap;

        public TrademarkLogic(
            NiisWebContext context,
            IWorkflowApplier<Domain.Entities.Request.Request> workflowApplier)
            : base(workflowApplier, context)
        {
            _logicMap = new Dictionary<string, Func<Domain.Entities.Request.Request, Expression<Func<DicRouteStage, bool>>>>
            {
                {DicDocumentType.Codes.ExpertRefusalOpinionFinal, FinalRejectionLogic},
                {DicDocumentType.Codes.ExpertTmRegisterRefusalOpinion, ExpertConclusionLogic},
                {DicDocumentType.Codes.ExpertTmRegisterOpinion, ExpertConclusionLogic},
                {DicDocumentType.Codes.ExpertTmRegistrationOpinionWithDisclaimer, ExpertConclusionLogic},
                {DicDocumentType.Codes.ExpertTmRegisterFinalOpinion, ExpertFinalConclusionLogic},
                {DicDocumentType.Codes.ExpertTmRegistrationFinalOpinionWithApplicantConsent, ExpertFinalConclusionLogic},
                {DicDocumentType.Codes.ExpertTmRegistrationFinalOpinionWithoutApplicantConsent, ExpertFinalConclusionLogic}
            };
        }
        
        public override async Task ApplyAsync(ApplicationUser user, RequestDocument requestDocument)
        {
            await ApplyStageAsync(GetStagePredicate(requestDocument), requestDocument.Request);
        }

        private Expression<Func<DicRouteStage, bool>> GetStagePredicate(RequestDocument rd)
        {
            return _logicMap.ContainsKey(rd.Document.Type.Code)
                ? _logicMap[rd.Document.Type.Code].Invoke(rd.Request)
                : null;
        }
        
        /// <summary>
        /// Логика обработки этапов при входящем документе "Экспертное заключение об отказе (окончательное)"
        /// </summary>
        /// <param name="request">Заявка</param>
        /// <returns>Запрос для получения этапа "Направлено заявителю заключение об окончательном отказе"</returns>
        private Expression<Func<DicRouteStage, bool>> FinalRejectionLogic(Domain.Entities.Request.Request request)
        {
            // На утверждение директору
            if (CurrentStageContains(request, "TM03.3.3.1"))
            {
                // Утверждено директором
                return s => s.Code.Equals("TM03.3.4");
            }

            return null;
        }

        /// <summary>
        ///     Логика обработки этапов при подписании исходящих документов "Экспертное заключение об отказе в регистрации ТЗ
        ///     (знака обслуживания)",
        ///     "Экспертное заключение о регистрации ТЗ", "ЭКСПЕРТНОЕ ЗАКЛЮЧЕНИЕ о регистрации ТЗ (знака обслуживания) с
        ///     дискламацией_частичным отказом"
        /// </summary>
        /// <param name="request">Заявка</param>
        /// <returns>Этап "На утверждение директору" или "Утверждено директором"</returns>
        private Expression<Func<DicRouteStage, bool>> ExpertConclusionLogic(Domain.Entities.Request.Request request)
        {
            // Вынесено экспертное заключение
            if (CurrentStageContains(request, "TM03.3.3"))
                // На утверждение директору
                return s => s.Code.Equals("TM03.3.3.1");
            
            // На утверждение директору
            if (CurrentStageContains(request, "TM03.3.3.1"))
            {
                // Утверждено директором
                return s => s.Code.Equals("TM03.3.4");
            }

            return null;
        }

        /// <summary>
        ///     Логика обработки этапов при подписании исходящего документа "ЭКСПЕРТНОЕ ЗАКЛЮЧЕНИЕ (окончательное) о регистрации ТЗ
        ///     (знака обслуживания)",
        ///     "ЭКСПЕРТНОЕ ЗАКЛЮЧЕНИЕ (окончательное) о регистрации ТЗ  с согласием заявителя", "ЭКСПЕРТНОЕ ЗАКЛЮЧЕНИЕ
        ///     (окончательное) о регистрации ТЗ  без согласия заявителя"
        /// </summary>
        /// <param name="request">Заявка</param>
        /// <returns>Этап "На утверждение директору" или "Утверждено директором"</returns>
        private Expression<Func<DicRouteStage, bool>> ExpertFinalConclusionLogic(Domain.Entities.Request.Request request)
        {
            // Окончательное экспертное заключение
            if (CurrentStageContains(request, "TM03.3.3.0"))
                // На утверждение директору
                return s => s.Code.Equals("TM03.3.3.1");

            // На утверждение директору
            if (CurrentStageContains(request, "TM03.3.3.1"))
                // Утверждено директором
                return s => s.Code.Equals("TM03.3.4");
            
            return null;
        }
    }
}