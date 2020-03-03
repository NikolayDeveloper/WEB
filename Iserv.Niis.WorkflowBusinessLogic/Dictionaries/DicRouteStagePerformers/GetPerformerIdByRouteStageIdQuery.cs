using System.Linq;
using Iserv.Niis.Domain.Entities.Dictionaries;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Queries;
using Iserv.Niis.Domain.Entities.Security;
using Microsoft.AspNetCore.Identity;


namespace Iserv.Niis.WorkflowBusinessLogic.Dictionaries.DicRouteStagePerformers
{
    /// <summary>
    /// Получения идентификатора исполнителя этапа по идентификатору этапа.
    /// </summary>
    public class GetPerformerIdByRouteStageIdQuery : BaseQuery
    {
        /// <summary>
        /// Выполнение запроса.
        /// </summary>
        /// <param name="routeStageId">Идентификатор этапа маршрута.</param>
        /// <returns>Идентификатор пользователя, ответственного за этап маршрута.</returns>
        public int? Execute(int routeStageId)
        {
            var dicRouteStagePerformerRepository = Uow.GetRepository<DicRouteStagePerformer>();
            var dicRouteStagePerformer = dicRouteStagePerformerRepository
                .AsQueryable()
                .Where(drsp => drsp.RouteStageId == routeStageId)
                .FirstOrDefault();

            if (dicRouteStagePerformer == null)
                return null;

            var userRepository = Uow.GetRepository<ApplicationUser>();
            var users = userRepository.AsQueryable();

            int? userId = users
                .Where(u => u.Id == dicRouteStagePerformer.UserId && 
                            u.IsDeleted == false)
                .Select(u => u.Id)
                .FirstOrDefault();

            return userId;

        }
    }
}
