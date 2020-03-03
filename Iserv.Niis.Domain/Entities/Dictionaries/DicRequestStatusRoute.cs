using Iserv.Niis.Domain.Entities.Dictionaries.DicMain;

namespace Iserv.Niis.Domain.Entities.Dictionaries
{
    /// <summary>
    /// Связь между статусом заявки и маршрутом.
    /// </summary>
    public class DicRequestStatusRoute
    {
        /// <summary>
        /// ID статуса заявки.
        /// </summary>
        public int DicRequestStatusId { get; set; }

        /// <summary>
        /// Статус заявки.
        /// </summary>
        public DicRequestStatus DicRequestStatus { get; set; }

        /// <summary>
        /// ID маршрута.
        /// </summary>
        public int DicRouteId { get; set; }

        /// <summary>
        /// Маршрут.
        /// </summary>
        public DicRoute DicRoute { get; set; }
    }
}
