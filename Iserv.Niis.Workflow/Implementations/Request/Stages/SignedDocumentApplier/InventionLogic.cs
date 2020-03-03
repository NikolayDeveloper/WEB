using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Iserv.Niis.DataAccess.EntityFramework;
using Iserv.Niis.Domain.Entities.Dictionaries;
using Iserv.Niis.Domain.Entities.Request;
using Iserv.Niis.Domain.Entities.Security;
using Iserv.Niis.Workflow.Abstract;

namespace Iserv.Niis.Workflow.Implementations.Request.Stages.SignedDocumentApplier
{
    public class InventionLogic : BaseLogic
    {
        private readonly Dictionary<string, Func<ApplicationUser, Domain.Entities.Request.Request, Expression<Func<DicRouteStage, bool>>>> _logicMap;

        public InventionLogic(
            NiisWebContext context,
            IWorkflowApplier<Domain.Entities.Request.Request> workflowApplier)
            : base(workflowApplier, context)
        {
            _logicMap = new Dictionary<string, Func<ApplicationUser, Domain.Entities.Request.Request, Expression<Func<DicRouteStage, bool>>>>
            {
                { DicDocumentType.Codes.ConclusionOfInventionPatentGrant, PatentGrantLogic },
                { DicDocumentType.Codes.ConclusionOfInventionPatentGrantRefuse, PatentGrantLogic }
            };
        }

        //TODO: Подписание не реализовано. Все логика подписания временно реализовано в классе TransferedDocumentApplier (при этапе "Контроль заместителя")
        public override async Task ApplyAsync(ApplicationUser user, RequestDocument requestDocument)
        {
            await ApplyStageAsync(GetStagePredicate(user, requestDocument), requestDocument.Request);
        }

        private Expression<Func<DicRouteStage, bool>> GetStagePredicate(ApplicationUser user, RequestDocument rd)
        {
            return _logicMap.ContainsKey(rd.Document.Type.Code)
                ? _logicMap[rd.Document.Type.Code].Invoke(user, rd.Request)
                : null;
        }

        /// <summary>
        /// Логика обработки этапов при исходящем документе "24_Заключение о выдаче патента на изобретение  (Форма ИЗ-3б)"
        /// или "27_Заключение об отказе в выдаче патента на изобретение (ОТРИЦАТЕЛЬНОЕ) (Форма ИЗ-4п)"
        /// </summary>
        /// <param name="user">Пользователь</param>
        /// <param name="request">Заявка</param>
        /// <returns>Запрос для получения этапа "Утверждено директором"</returns>
        private Expression<Func<DicRouteStage, bool>> PatentGrantLogic(ApplicationUser user, Domain.Entities.Request.Request request)
        {
            if (new[] { "024", "041" }.Contains(user.Position.Code))
            {
                if (CurrentStageContains(request, "B03.3.2") 
                    && (AnyDocuments(request, DicDocumentType.Codes.ConclusionOfInventionPatentGrant)
                    || AnyDocuments(request, DicDocumentType.Codes.ConclusionOfInventionPatentGrantRefuse)))
                {
                    return s => s.Code.Equals("B03.3.2.1");
                }

                if (CurrentStageContains(request, "B03.3.2")
                    && (AnyDocuments(request, DicDocumentType.Codes.ConclusionOfInventionPatentGrant)
                        || AnyDocuments(request, DicDocumentType.Codes.ConclusionOfInventionPatentGrantRefuse)))
                {
                    return s => s.Code.Equals("B03.3.2.1");
                }
            }

            if (new[] { "001", "01.1_F" }.Contains(user.Position.Code))
            {
                if (CurrentStageContains(request, "B03.3.2.1"))
                {
                    return s => s.Code.Equals("B03.3.3");
                }
            }

            return null;
        }
    }
}