using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Iserv.Niis.Domain.Entities.Security;

namespace Iserv.Niis.Domain.Entities.Dictionaries
{
    /// <summary>
    /// Исполнители на этапах маршрута
    /// </summary>
    public class DicRouteStagePerformer
    {
        public int Id { get; set; }

        /// <summary>
        /// Этап маршрута
        /// </summary>
        public int RouteStageId { get; set; }
        public DicRouteStage RouteStage { get; set; }

        /// <summary>
        /// Пользователь
        /// </summary>
        public int UserId { get; set; }
        public ApplicationUser User { get; set; }
    }
}
