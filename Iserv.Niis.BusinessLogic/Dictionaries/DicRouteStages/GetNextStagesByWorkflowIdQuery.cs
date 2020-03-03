using Iserv.Niis.Common.Codes;
using Iserv.Niis.Domain.Entities.Dictionaries;
using Iserv.Niis.Domain.Entities.Document;
using Iserv.Niis.Domain.Entities.Other;
using Microsoft.EntityFrameworkCore;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Queries;
using NetCoreDataAccess.Repository;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Iserv.Niis.BusinessLogic.Dictionaries.DicRouteStages
{
    /// <summary>
    /// Запрос, который возвращает массив следующих доступных этапов бизнес процесса документа по идентификатору его текущего бизнес процесса.
    /// </summary>
    public class GetDocumentNextStagesByDocumentCurrentWorkflowIdQuery : BaseQuery
    {
        /// <summary>
        /// Выполнение запроса.
        /// </summary>
        /// <param name="documentCurrentWorkflowId">Идентификатор текущего бизнес процесса документа.</param>
        /// <returns>Массив следующих доступных этапов бизнес процесса.</returns>
        public async Task<DicRouteStage[]> ExecuteAsync(int documentCurrentWorkflowId)
        {
            // Получаем текущий бизнес процесс документа и документ с его типом, связанный с этим бизнес процессом. 
            DocumentWorkflow currentDocumentWorkflow = await GetDocumentWorkflowWithOwnerAndItsType(documentCurrentWorkflowId);
            Document document = currentDocumentWorkflow.Owner;

            // Получаем массив всех следующих маршрутов, по текущому бизнес процессу документа.
            RouteStageOrder[] allNextRouteStageOrders = await GetAllNextRouteStageOrders(currentDocumentWorkflow);

            // Фильтруем все маршруты и получаем маршруты, у которых есть классификаторы.
            RouteStageOrder[] nextRouteStageOrdersWithClassifications = allNextRouteStageOrders
                .Where(routeStageOrder => routeStageOrder.ClassificationId.HasValue)
                .ToArray();
        
            // Создаем массив доступных маршрутов и записываем туда маршруты, классификаторы которых совпадают с классификатором документа.
            RouteStageOrder[] nextStageOrders = nextRouteStageOrdersWithClassifications
                .Where(routeStageOrder => routeStageOrder.ClassificationId == document.Type.ClassificationId)
                .ToArray();

            // Если маршрутов с классификаторами не найдено, то в доступные маршруты записываются все маршруты без классификаторов.
            if (!nextStageOrders.Any())
            {
                RouteStageOrder[] nextRouteStageOrdersWithoutClassifications = allNextRouteStageOrders
                    .Where(routeStageOrder => !routeStageOrder.ClassificationId.HasValue)
                    .ToArray();

                nextStageOrders = nextRouteStageOrdersWithoutClassifications;
            }

            // Получаем идентификаторы следующих этапов.
            IEnumerable<int> nextStageIds = nextStageOrders.Select(routeStageOrder => routeStageOrder.NextStageId);

            // Получаем следующие этапы бизнес процесса.
            DicRouteStage[] nextRouteStages = await GetRouteStagesByIds(nextStageIds);

            // Фильтруем следующие этапы, используя бизнес логику.
            nextRouteStages = await FilterRouteStagesUsingBusinessLogic(currentDocumentWorkflow, nextRouteStages);            

            return nextRouteStages;
        }

        // Внимание! Желательно всякие странные условия, связанные получением следующих этапов бизнесс процесса документов,
        // которые по разным причинам нельзя реализовать на уровне базы данных, встраивать в этот метод, не загрязняя код в контроллерах или других местах.

        /// <summary>
        /// Фильтрует следующие этапы бизнес процесса, используя какую-то бизнес логику, которая не реализована на уровне бд.
        /// </summary>
        /// <param name="currentDocumentWorkflow">Текущий бизнес процесс документа.</param>
        /// <param name="nextRouteStages">Массив следующих этапов бизнес процесса документа.</param>
        /// <returns>Отфильтрованный массив следующих этапов бизнес процесса документа.</returns>
        private async Task<DicRouteStage[]> FilterRouteStagesUsingBusinessLogic(DocumentWorkflow currentDocumentWorkflow, DicRouteStage[] nextRouteStages)
        {
            Document document = currentDocumentWorkflow.Owner;

            // Если документ является свидетельством на ТЗ или НМПТ и его текущий этап бизнес процесса "Создание исходящего документа", то 
            // он отправляет на этап "Контроль заместителя начальника департамента", если этот этап был в массиве следующих доступных этапов.
            // Если же документ является отчетом эксперта по заявке ТЗ и НМПТ и его текущий этап бизнес процесса "Создание внутреннего документа", то
            // доступных этапов для перехода у этого документа не будет.
            if (document.Type.Code == DicDocumentTypeCodes.NmptCertificate)
            {
                DicRouteStage fromStage = await GetRouteStageById(currentDocumentWorkflow.CurrentStageId ?? 0);

                if (fromStage.Code == RouteStageCodes.DocumentOutgoing_01_1)
                {
                    nextRouteStages = nextRouteStages
                        .Where(routeStage => routeStage.Code == RouteStageCodes.OUT_DeputyHeadOfDepartment)
                        .ToArray();
                }
            }
            else if (document.Type.Code == DicDocumentTypeCodes.TZPOL555 ||
                     document.Type.Code == DicDocumentTypeCodes.TZPOL555PR ||
                     document.Type.Code == DicDocumentTypeCodes.TZPOL555PRWD ||
                     document.Type.Code == DicDocumentTypeCodes.TZPOL555PF)
            {
                DicRouteStage fromStage = await GetRouteStageById(currentDocumentWorkflow.CurrentStageId ?? 0);

                if (fromStage.Code == RouteStageCodes.DocumentInternal_0_1)
                {
                    nextRouteStages = new DicRouteStage[] { };
                }
            }
            

            return nextRouteStages;
        }

        /// <summary>
        /// Получает этап бизнес процесса документа по его идентификатору.
        /// </summary>
        /// <param name="routeStageId">Идентификатор этапа бизнес процесса.</param>
        /// <returns>Этап бизнес процесса.</returns>
        private async Task<DicRouteStage> GetRouteStageById(int routeStageId)
        {
            IRepository<DicRouteStage> dicRouteStageRepository = Uow.GetRepository<DicRouteStage>();

            return await dicRouteStageRepository.GetByIdAsync(routeStageId);
        }

        /// <summary>
        /// Получает массив этапов бизнес процессов документа по их идентификаторам.
        /// </summary>
        /// <param name="routeStageIds">Идентификаторы этапов бизнес процессов.</param>
        /// <returns>Массив этапов бизнес процесса документа.</returns>
        private async Task<DicRouteStage[]> GetRouteStagesByIds(IEnumerable<int> routeStageIds)
        {
            IRepository<DicRouteStage> dicRouteStageRepository = Uow.GetRepository<DicRouteStage>();

            return await dicRouteStageRepository
                .AsQueryable()
                .Where(routeStage => routeStageIds.Contains(routeStage.Id))
                .ToArrayAsync();
        }

        /// <summary>
        /// Получает массив всех доступных маршрутов документа по его текущему бизнес процессу.
        /// </summary>
        /// <param name="currentDocumentWorkflow">Текущий бизнес процесс документа.</param>
        /// <returns>Массив доступных маршрутов документа.</returns>
        private async Task<RouteStageOrder[]> GetAllNextRouteStageOrders(DocumentWorkflow currentDocumentWorkflow)
        {
            IRepository<RouteStageOrder> routeStageOrderRepository = Uow.GetRepository<RouteStageOrder>();

            return await routeStageOrderRepository
                .AsQueryable()
                .Where(routeStageOrder => routeStageOrder.CurrentStageId == currentDocumentWorkflow.CurrentStageId && !routeStageOrder.IsAutomatic)
                .ToArrayAsync();
        }

        /// <summary>
        /// Получает бизнес процесс документа по его идентификатору вместе со связанным с ним документом и его типом.
        /// </summary>
        /// <param name="documentWorkflowId">Идентификатор бизнес процесса документа.</param>
        /// <returns>Бизнес процесс документа.</returns>
        private async Task<DocumentWorkflow> GetDocumentWorkflowWithOwnerAndItsType(int documentWorkflowId)
        {
            IRepository<DocumentWorkflow> documentWorkflowRepository = Uow.GetRepository<DocumentWorkflow>();

            return await documentWorkflowRepository
                .AsQueryable()
                .Include(workflow => workflow.Owner)
                    .ThenInclude(document => document.Type)
                .FirstOrDefaultAsync(workflow => workflow.Id == documentWorkflowId);
        }
    }
}