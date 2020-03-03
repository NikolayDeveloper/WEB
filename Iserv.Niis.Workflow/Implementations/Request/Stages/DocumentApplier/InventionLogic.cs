using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Iserv.Niis.DataAccess.EntityFramework;
using Iserv.Niis.Domain.Entities.Dictionaries;
using Iserv.Niis.Domain.Entities.Request;
using Iserv.Niis.Workflow.Abstract;
using Microsoft.EntityFrameworkCore;

namespace Iserv.Niis.Workflow.Implementations.Request.Stages.DocumentApplier
{
    public class InventionLogic : BaseLogic
    {
        private readonly NiisWebContext _context;
        private readonly Dictionary<string, Func<Domain.Entities.Request.Request, Expression<Func<DicRouteStage, bool>>>> _logicMap;

        public InventionLogic(
            NiisWebContext context,
            IWorkflowApplier<Domain.Entities.Request.Request> workflowApplier)
            : base(workflowApplier, context)
        {
            _context = context;
            _logicMap = new Dictionary<string, Func<Domain.Entities.Request.Request, Expression<Func<DicRouteStage, bool>>>>
            {
                { DicDocumentType.Codes.PetitionOfApplicationRevocation, RevocationLogic },
                { DicDocumentType.Codes.PetitionForExtendTimeRorResponse, ProlongationLogic },
                { DicDocumentType.Codes.AnswerToRequest, ExaminationLogic },
                { DicDocumentType.Codes.PetitionForRestoreTime, RestorationLogic },
                { DicDocumentType.Codes.PetitionForSuspensionOfOfficeWork, SuspensionLogic },
                { DicDocumentType.Codes.Objection, ObjectionLogic },
                { DicDocumentType.Codes.PetitionForExtendTimeRorObjections, ObjectionProlongationLogic },
                { DicDocumentType.Codes.DecisionOfAppealsBoard, PreparationToGosRegisterTransferLogic },
                { DicDocumentType.Codes.DecisionOfAuthorizedBody, ReturnFromMinistryLogic },
                { DicDocumentType.Codes.ConclusionOfInventionPatentGrantRefuse, PatentRefuseLogic },
            };
        }

        public override async Task ApplyAsync(RequestDocument requestDocument)
        {
            await ApplyStageAsync(GetStagePredicate(requestDocument), requestDocument.Request);
        }

        /// <summary>
        /// Логика обработки этапов при входящем документе "Решение уполномоченного органа"
        /// </summary>
        /// <param name="request">Заявка</param>
        /// <returns>Запрос для получения этапа "Возвращено с МЮ РК" или "Направлено уведомление о принятии решения"</returns>
        private Expression<Func<DicRouteStage, bool>> ReturnFromMinistryLogic(Domain.Entities.Request.Request request)
        {
            if (CurrentStageContains(request, "B03.3.4"))
            {
                return s => s.Code.Equals("B03.3.5");
            }

            return null;
        }

        private Expression<Func<DicRouteStage, bool>> PatentRefuseLogic(Domain.Entities.Request.Request request)
        {
            if (CurrentStageContains(request, "B03.3.4.1.0"))
            {
                return s => s.Code.Equals("B03.3.9");
            }

            return null;
        }

        /// <summary>
        /// Логика обработки этапов при входящем документе "Решение Апелляционного совета"
        /// </summary>
        /// <param name="request">Заявка</param>
        /// <returns>Запрос для получения этапа "Подготовка для передачи в Госреестр"</returns>
        private Expression<Func<DicRouteStage, bool>> PreparationToGosRegisterTransferLogic(Domain.Entities.Request.Request request)
        {
            if (CurrentStageContains(request, "B03.3.4.1.0"))
            {
                if (AnyDocuments(request, DicDocumentType.Codes.NotificationOfDecisionPatentGrant))
                {
                    return s => s.Code.Equals("B03.3.7.1");
                }
            }

            return null;
        }

        /// <summary>
        /// Логика обработки этапов при входящем документе "Ходатайство о продлении срока подачи возражения "
        /// </summary>
        /// <param name="request">Заявка</param>
        /// <returns>Запрос для получения этапа "Продление срока"</returns>
        private Expression<Func<DicRouteStage, bool>> ObjectionProlongationLogic(Domain.Entities.Request.Request request)
        {
            if (CurrentStageContains(request, "B03.3.4.1"))
            {
                return s => s.Code.Equals("B03.3.1.1.0");
            }

            return null;
        }

        /// <summary>
        /// Логика обработки этапов при входящем документе "Возражения"
        /// </summary>
        /// <param name="request">Заявка</param>
        /// <returns>Запрос для получения этапа "Возражение на предварительный или частичный отказ"B03.3.4.1.0</returns>
        private Expression<Func<DicRouteStage, bool>> ObjectionLogic(Domain.Entities.Request.Request request)
        {
            if (CurrentStageContains(request, "B03.3.4.1", "B03.3.1.1.0") 
                && (AnyDocuments(request, DicDocumentType.Codes.ConclusionOfInventionPatentGrant) 
                || AnyDocuments(request, DicDocumentType.Codes.ConclusionOfInventionPatentGrantRefuse)))
            {
                return s => s.Code.Equals("B03.3.4.1.0");
            }

            return null;
        }

        /// <summary>
        /// Логика обработки этапов при входящем документе "Ходатайство о приостановлении делопроизводства"
        /// </summary>
        /// <param name="request">Заявка</param>
        /// <returns>Запрос для получения этапа "Делопроизводство приостановлено"</returns>
        private Expression<Func<DicRouteStage, bool>> SuspensionLogic(Domain.Entities.Request.Request request)
        {
            if (request.ProtectionDocId == null)
            {
                return s => s.Code.Equals("B03.3.9.0");
            }

            return null;
        }

        private Expression<Func<DicRouteStage, bool>> GetStagePredicate(RequestDocument rd)
        {
            return _logicMap.ContainsKey(rd.Document.Type.Code)
                ? _logicMap[rd.Document.Type.Code].Invoke(rd.Request)
                : null;
        }

        /// <summary>
        /// Логика обработки этапов при входящем документе "Ходатайство о восстановлении пропущенного срока"
        /// </summary>
        /// <param name="request">Заявка</param>
        /// <returns>Запрос для получения этапа "Готовые для передачи на полную экспертизу" или "Предварительная экспертиза" или "Полная экспертиза" или "Готовые для передачи в Госреестр"</returns>
        private Expression<Func<DicRouteStage, bool>> RestorationLogic(Domain.Entities.Request.Request request)
        {
            if (!HasPaidInvoices(request, DicTariff.Codes.RequestAnswerMissedTimeRestoration)) return null;

            if (FromStageContains(request, "B02.2.1.0.0") && CurrentStageContains(request, "B03.3.1.1")
                && HasPaidInvoices(request, DicTariff.Codes.ExtensionAndRestorationTimesForAnswerToExaminationRequest)
                && AnyDocuments(request, DicDocumentType.Codes.AnswerToRequest))
            {
                return s => s.Code.Equals("B03.2.1");
            }

            if (CurrentStageContains(request, "B03.3.1.1.1") && HasPaidInvoices(request,
                    DicTariff.Codes.ExaminationOfApplicationForInventionMerits,
                    DicTariff.Codes.AcceleratedExaminationOfApplicationForInventionMerits,
                    DicTariff.Codes.ExaminationOfApplicationForInventionMeritsWithInternationalReport,
                    DicTariff.Codes.ExaminationOfApplicationForInventionMeritsWithSearchReport)
                    && HasPaidInvoices(request, DicTariff.Codes.ExtensionAndRestorationTimesForAnswerToExaminationRequest))
            {
                return s => s.Code.Equals("B03.2.1.1");
            }

            if (CurrentStageContains(request, "B03.3.1.1.1")
                && AnyDocuments(request, DicDocumentType.Codes.AnswerToRequest)
                && HasPaidInvoices(request, DicTariff.Codes.ExtensionAndRestorationTimesForAnswerToExaminationRequest))
            {
                return s => s.Code.Equals("B03.2.4");
            }

            return null;
        }

        /// <summary>
        /// Логика обработки этапов при входящем документе "Ответ на запрос"
        /// </summary>
        /// <param name="request">Заявка</param>
        /// <returns>Запрос для получения этапа "Формальная экспертиза" или "Полная экспертиза"</returns>
        private Expression<Func<DicRouteStage, bool>> ExaminationLogic(Domain.Entities.Request.Request request)
        {
            if (FromStageContains(request, "B02.2.1.0.0"))
            {
                if (CurrentStageContains(request, "B03.3.1.1.0"))
                {
                    return s => s.Code.Equals("B03.2.1");
                }

                if (CurrentStageContains(request, "B03.3.1.1.1") && HasPaidInvoices(request, DicTariff.Codes.ExtensionAndRestorationTimesForAnswerToExaminationRequest) &&
                    AnyDocuments(request, DicDocumentType.Codes.PetitionForRestoreTime))
                {
                    return s => s.Code.Equals("B03.2.1");
                }
            }

            if (FromStageContains(request, "B03.3.1.1") && CurrentStageContains(request, "B03.3.1.1.0"))
            {
                var beforeStageFormal = _context.RequestWorkflows.Include(rw => rw.FromStage)
                    .LastOrDefault(w => w.FromStage.Code.Equals("B03.2.1") && w.CurrentStageId == request.CurrentWorkflow.FromStageId && w.OwnerId == request.Id);
                var beforeStageFull = _context.RequestWorkflows.Include(rw => rw.FromStage)
                    .LastOrDefault(w => w.FromStage.Code.Equals("B03.2.4") && w.CurrentStageId == request.CurrentWorkflow.FromStageId && w.OwnerId == request.Id);
                if (beforeStageFormal != null)
                {
                    if (beforeStageFull != null)
                    {
                        if (beforeStageFormal.DateCreate > beforeStageFull.DateCreate)
                        {
                            return s => s.Code.Equals("B03.2.1");
                        }
                    }
                    else
                    {
                        return s => s.Code.Equals("B03.2.1");
                    }
                }
                if (beforeStageFull != null)
                {
                    if (beforeStageFormal != null)
                    {
                        if (beforeStageFormal.DateCreate < beforeStageFull.DateCreate)
                        {
                            return s => s.Code.Equals("B03.2.4");
                        }
                    }
                    else
                    {
                        return s => s.Code.Equals("B03.2.4");
                    }
                }
            }

            if (FromStageContains(request, "B03.2.4") && CurrentStageContains(request, "B03.3.1.1"))
            {
                return s => s.Code.Equals("B03.2.4");
            }

            if (FromStageContains(request, "B03.2.1") && CurrentStageContains(request, "B03.3.1.1"))
            {
                return s => s.Code.Equals("B03.2.1");
            }

            if (CurrentStageContains(request, "B03.3.1.1.1") 
                && AnyDocuments(request, DicDocumentType.Codes.PetitionForRestoreTime)
                && HasPaidInvoices(request, DicTariff.Codes.ExtensionAndRestorationTimesForAnswerToExaminationRequest))
            {
                return s => s.Code.Equals("B03.2.4");
            }

            return null;
        }

        /// <summary>
        /// Логика обработки этапов при входящем документе "Ходатайство о продлении срока ответа на запрос"
        /// </summary>
        /// <param name="request">Заявка</param>
        /// <returns>Запрос для получения этапа "Продление срока"</returns>
        private Expression<Func<DicRouteStage, bool>> ProlongationLogic(Domain.Entities.Request.Request request)
        {
            if (CurrentStageContains(request, "B02.2") && HasPaidInvoices(request,
                    DicTariff.Codes.ExtensionAndRestorationTimesForAnswerToExaminationRequest)
                && (AnyDocuments(request, DicDocumentType.Codes.NotificationForInventionPaymentForIndividuals)
                || AnyDocuments(request, DicDocumentType.Codes.NotificationForInventionPaymentForBeneficiaries)))
            {
                return s => s.Code.Equals("B02.2.0.0");
            }

            if (CurrentStageContains(request, "B03.3.1.1") && HasPaidInvoices(request,
                    DicTariff.Codes.ExtensionAndRestorationTimesForAnswerToExaminationRequest))
            {
                return s => s.Code.Equals("B03.3.1.1.0");
            }
            
            return null;
        }

        
        /// <summary>
        /// Логика обработки этапов при входящем документе "Ходатайство об отзыве по просьбе заявителя"
        /// </summary>
        /// <param name="request">Заявка</param>
        /// <returns>Запрос для получения этапа "Отозвана"</returns>
        private Expression<Func<DicRouteStage, bool>> RevocationLogic(Domain.Entities.Request.Request request)
        {
            if (request.ProtectionDocId == null)
            {
                return s => s.Code.Equals("B04.0");
            }

            return null;
        }
    }
}
