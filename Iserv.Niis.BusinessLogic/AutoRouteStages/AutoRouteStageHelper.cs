using Iserv.Niis.BusinessLogic.Workflows.Documents;
using Iserv.Niis.DI;
using Iserv.Niis.Domain.Entities.AutoRouteStages;
using Iserv.Niis.Domain.Entities.Document;
using Iserv.Niis.Domain.Entities.Security;
using Iserv.Niis.Model.Models.Material;
using Microsoft.EntityFrameworkCore;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; ;
using NetCoreDataAccess.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Iserv.Niis.BusinessLogic.AutoRouteStages
{
    public class AutoRouteStageHelper : IAutoRouteStageHelper
    {
        private readonly IUnitOfWork _unitOfWork;

        public AutoRouteStageHelper(
            IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        private IExecutor _executor;
        protected IExecutor Executor => _executor ?? (_executor = NiisAmbientContext.Current.Executor);

        /// <summary>
        /// Метод для автоматичесскиго перехода на этап в зависимости от конфигураций
        /// </summary>
        /// <param name="documentId">Id Документа</param>
        /// <returns>Создание Workflow</returns>
        public async Task<IList<MaterialWorkflowDto>> StartAutuRouteStage(int documentId, bool isSimpleWf = false)
        {
            var documentWorkflow = _unitOfWork.GetRepository<DocumentWorkflow>();
            var document = _unitOfWork.GetRepository<Document>()
                .AsQueryable()
                .Include(cw => cw.Requests).ThenInclude(cw => cw.Request)
                                           .ThenInclude(cw => cw.CurrentWorkflow)
                .Include(cw => cw.Workflows)
                .Include(cw => cw.Type)
                .FirstOrDefault(d => d.Id == documentId);
            if (document == null) return null;

            var currentUserId = NiisAmbientContext.Current.User.Identity.UserId;

            var currentWorkflow = document.CurrentWorkflows.FirstOrDefault(d => d.CurrentUserId == currentUserId);
            if (currentWorkflow == null)
            {
                if (isSimpleWf)
                    currentWorkflow = document.CurrentWorkflows.FirstOrDefault();
                else
                    return null;
            }
            if (currentWorkflow == null) return null;

            //Загружаем автопереходы связанные с текущим этапом(должен быть один)
            var autoRouteStages = _unitOfWork.GetRepository<AutoRouteStage>()
            .AsQueryable()
            .Include(d => d.AutoRouteStageEvents)
            .Include(d => d.NextStage)
            .Include(cw => cw.AutoRouteStageViewers)
            .Where(d => d.CurrentStageId == currentWorkflow.CurrentStageId
                && d.AutoRouteStageEvents.Any(e => e.TypeId == document.TypeId))
            .ToList();

            if (!autoRouteStages.Any()) return null;

            var nextStages = new List<MaterialWorkflowDto>();

            foreach (var autoRouteStage in autoRouteStages)
            {
                //Берем Тип документа и Должность для перехода(должен быть один)
                var stageEvent = autoRouteStage.AutoRouteStageEvents
                    .Where(d => d.TypeId == document.TypeId);
                if (!stageEvent.Any()) continue;

                var autoExecutor = _unitOfWork.GetRepository<ApplicationUser>()
                        .AsQueryable()
                        .FirstOrDefault(d => d.PositionId == stageEvent.FirstOrDefault().PositionId && d.IsDeleted == false);
                if (autoExecutor == null) continue;

                var workflowDto = new MaterialWorkflowDto()
                {
                    OwnerId = document.Id,
                    IsCurent = true,
                    DateCreate = DateTimeOffset.Now,
                    FromStageId = currentWorkflow.CurrentStageId,
                    PreviousWorkflowId = currentWorkflow.Id,
                    CurrentStageId = autoRouteStage.NextStageId,
                    FromUserId = currentWorkflow.CurrentUserId,
                    CurrentUserId = autoExecutor.Id,
                    CurrentUserDepartmentId = autoExecutor.DepartmentId,
                    IsComplete = autoRouteStage.NextStage.IsLast,
                    RouteId = currentWorkflow.RouteId,
                };

                var result = await Executor.GetHandler<CreateMaterialWorkFlowHandler>().Process(r => r.ExecuteAsync(workflowDto));
                nextStages.Add(result);
            }
            if (!nextStages.Any()) return null;

            return nextStages;
        }   
    }
}
