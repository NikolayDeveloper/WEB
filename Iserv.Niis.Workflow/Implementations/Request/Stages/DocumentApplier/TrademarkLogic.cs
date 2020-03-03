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
    public class TrademarkLogic : BaseLogic
    {
        private readonly Dictionary<string, Func<Domain.Entities.Request.Request, Expression<Func<DicRouteStage, bool>>>> _logicMap;
        private readonly NiisWebContext _context;

        public TrademarkLogic(
            NiisWebContext context,
            IWorkflowApplier<Domain.Entities.Request.Request> workflowApplier)
            : base(workflowApplier, context)
        {
            _context = context;
            _logicMap = new Dictionary<string, Func<Domain.Entities.Request.Request, Expression<Func<DicRouteStage, bool>>>>
            {
                { DicDocumentType.Codes.PetitionOfApplicationRevocation, RevocationLogic },
                { DicDocumentType.Codes.PetitionForSuspensionOfOfficeWork, SuspensionLogic },
                { DicDocumentType.Codes.PetitionForExtendTimeRorResponse, ProlongationLogic },
                { DicDocumentType.Codes.PetitionForRestoreTime, RestorationLogic },
                { DicDocumentType.Codes.AnswerToRequest, ExaminationLogic },
                { DicDocumentType.Codes.Objection, ObjectionLogic },
                { DicDocumentType.Codes.PetitionOfApplicantConsent, AgreementLogic },
                { DicDocumentType.Codes.DecisionOfAuthorizedBody, ReturnFromMinistryLogic },
                { DicDocumentType.Codes.DecisionOfAppealsBoard, PreparationToGosRegisterTransferLogic }
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

        /// <summary>
        /// Логика обработки этапов при входящем документе "Решение Апелляционного совета"
        /// </summary>
        /// <param name="request">Заявка</param>
        /// <returns>Запрос для получения этапа "Решение Апелляционного совета"</returns>
        private Expression<Func<DicRouteStage, bool>> PreparationToGosRegisterTransferLogic(Domain.Entities.Request.Request request)
        {
            // Апелляционный Совет
            if (CurrentStageContains(request, "TM03.3.9.2") && AnyDocuments(request, DicDocumentType.Codes.DecisionOfAppealsBoard))
            {
                // Решение Апелляционного совета
                return s => s.Code.Equals("TM03.3.9.2.0");
            }
            
            return null;
        }

        /// <summary>
        /// Логика обработки этапов при входящем документе "Решение уполномоченного органа"
        /// </summary>
        /// <param name="request">Заявка</param>
        /// <returns>Запрос для получения этапа "Возвращено с МЮ РК" или "Подготовка для передачи в Госреестр"</returns>
        private Expression<Func<DicRouteStage, bool>> ReturnFromMinistryLogic(Domain.Entities.Request.Request request)
        {
            // Передано в МЮ РК
            if (CurrentStageContains(request, "TM03.3.5"))
            {
                // Возвращено с МЮ РК
                return s => s.Code.Equals("TM03.3.6");
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
                return s => s.Code.Equals("TM02.2.3");
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
                return s => s.Code.Equals("TM03.3.9.0");
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
            if (CurrentStageContains(request, "TM03.3.7.0", "TM03.3.4.4", "TM03.3.4.4.0") &&
                HasPaidInvoices(request, DicTariff.Codes.RequestAnswerTimeExtensionForMonth))
            {
                return s => s.Code.Equals("TM03.3.7.3");
            }

            var invoiceDate = _context.PaymentInvoices.Include(pi => pi.Tariff)
                .LastOrDefault(pi =>
                    pi.RequestId == request.Id &&
                    pi.Tariff.Code.Contains(DicTariff.Codes.RequestAnswerTimeExtensionForMonth) &&
                    pi.Status.Code != "notpaid")?.DateCreate;
            if (invoiceDate == null)
            {
                return null;
            }

            var stageDate = _context.RequestWorkflows.Include(rw => rw.CurrentStage)
                .LastOrDefault(rw => rw.CurrentStage.Code.Equals("TM03.3.7.1") && rw.OwnerId == request.Id)
                ?.DateCreate;
            if (stageDate == null)
            {
                return null;
            }

            if (CurrentStageContains(request, "TM03.3.7.1") &&
                HasPaidInvoices(request, DicTariff.Codes.RequestAnswerTimeExtensionForMonth) &&
                invoiceDate.Value > stageDate.Value)
            {
                return s => s.Code.Equals("TM03.3.7.3");
            }

            return null;
        }

        /// <summary>
        /// Логика обработки этапов при входящем документе "Ходатайство о восстановлении пропущенного срока"
        /// </summary>
        /// <param name="request">Заявка</param>
        /// <returns>Запрос для получения этапа "Готовые для передачи на полную экспертизу" или "Предварительная экспертиза" или "Полная экспертиза" или "Готовые для передачи в Госреестр"</returns>
        private Expression<Func<DicRouteStage, bool>> RestorationLogic(Domain.Entities.Request.Request request)
        {
            if (!HasPaidInvoices(request, DicTariff.Codes.RequestAnswerMissedTimeRestoration)) return null;

            if (FromStageContains(request, "TM03.2.1") && CurrentStageContains(request, "TM03.3.7.0") &&
                AnyDocuments(request, DicDocumentType.Codes.AnswerToRequest))
            {
                return s => s.Code.Equals("TM03.2.2");
            }

            if (FromStageContains(request, "TM03.2.2") && CurrentStageContains(request, "TM03.3.7.0") &&
                HasPaidInvoices(request, DicTariff.Codes.TmRequestExamination, DicTariff.Codes.TmRequestExaminationWithThreeOrMoreClass))
            {
                return s => s.Code.Equals("TM03.2.2.1");
            }

            if (FromStageContains(request, "TM03.3.1") && CurrentStageContains(request, "TM03.3.7.0") &&
                AnyDocuments(request, DicDocumentType.Codes.AnswerToRequest))
            {
                return s => s.Code.Equals("TM03.3.2");
            }

            if (CurrentStageContains(request, "TM03.3.7.0") && AnyDocuments(request, DicDocumentType.Codes.Objection))
            {
                return s => s.Code.Equals("TM03.3.2");
            }

            // Ожидание восстановления пропущенного срока
            if (CurrentStageContains(request, "TM03.3.7.0") && HasPaidInvoices(request, DicTariff.Codes.TmNmptRegistration))
            {
                // Готовые для передачи в Госреестр
                return s => s.Code.Equals("TM03.3.8");
            }

            return null;
        }
        
        /// <summary>
        /// Логика обработки этапов при входящем документе "Ответ на запрос"
        /// </summary>
        /// <param name="request">Заявка</param>
        /// <returns>Запрос для получения этапа "Предварительная экспертиза" или "Полная экспертиза"</returns>
        private Expression<Func<DicRouteStage, bool>> ExaminationLogic(Domain.Entities.Request.Request request)
        {
            var formalExamDate = request.Workflows.LastOrDefault(w => w.CurrentStage.Code.Equals("TM03.2.2"))?.DateCreate ?? DateTimeOffset.MinValue;
            var fullExamDate = request.Workflows.LastOrDefault(w => w.CurrentStage.Code.Equals("TM03.3.2"))?.DateCreate ?? DateTimeOffset.MinValue;
            
            if (formalExamDate > fullExamDate)
            {
                if (CurrentStageContains(request, "TM03.3.7.1", "TM03.3.7.3"))
                {
                    return s => s.Code.Equals("TM03.2.2");
                }

                if (CurrentStageContains(request, "TM03.3.7.0") && HasPaidInvoices(request, DicTariff.Codes.RequestAnswerMissedTimeRestoration) &&
                    AnyDocuments(request, DicDocumentType.Codes.PetitionForRestoreTime))
                {
                    return s => s.Code.Equals("TM03.2.2");
                }
            }

            if (formalExamDate < fullExamDate)
            {
                if (CurrentStageContains(request, "TM03.3.7.1", "TM03.3.7.3"))
                {
                    return s => s.Code.Equals("TM03.3.2");
                }

                if (CurrentStageContains(request, "TM03.3.7.0") && HasPaidInvoices(request, DicTariff.Codes.RequestAnswerMissedTimeRestoration) &&
                    AnyDocuments(request, DicDocumentType.Codes.PetitionForRestoreTime))
                {
                    return s => s.Code.Equals("TM03.3.2");
                }
            }

            return null;
        }

        /// <summary>
        /// Логика обработки этапов при входящем документе "Возражения"
        /// </summary>
        /// <param name="request">Заявка</param>
        /// <returns>Запрос для получения этапа "Возражение на предварительный или частичный отказ" или "Апелляционный Совет"</returns>
        private Expression<Func<DicRouteStage, bool>> ObjectionLogic(Domain.Entities.Request.Request request)
        {
            if (CurrentStageContains(request, "TM03.3.4.4", "TM03.3.4.4.0", "TM03.3.7.3"))
            {
                return s => s.Code.Equals("TM03.3.2.0");
            }

            if (CurrentStageContains(request, "TM03.3.7.0") && HasPaidInvoices(request, DicTariff.Codes.RequestAnswerMissedTimeRestoration) &&
                AnyDocuments(request, DicDocumentType.Codes.PetitionForRestoreTime))
            {
                return s => s.Code.Equals("TM03.3.2.0");
            }

            // Направлено заявителю заключение об окончательном отказе
            if (CurrentStageContains(request, "TM03.3.9.1"))
            {
                // Апелляционный Совет
                return s => s.Code.Equals("TM03.3.9.2");
            }

            return null;
        }

        /// <summary>
        /// Логика обработки этапов при входящем документе "Ходатайство о согласии заявителя с экспертным заключением"
        /// </summary>
        /// <param name="request">Заявка</param>
        /// <returns>Запрос для получения этапа "Вынесение окончательного экспертного заключения (с согласием/без согласия/по решению МЮ)"</returns>
        private Expression<Func<DicRouteStage, bool>> AgreementLogic(Domain.Entities.Request.Request request)
        {
            if (CurrentStageContains(request, "TM03.3.4.4", "TM03.3.7.3", "TM03.3.7.0") &&
                HasPaidInvoices(request, DicTariff.Codes.RequestAnswerMissedTimeRestoration) && AnyDocuments(request, DicDocumentType.Codes.PetitionForRestoreTime))
            {
                return s => s.Code.Equals("TM03.3.2.0.0");
            }

            return null;
        }
    }
}
