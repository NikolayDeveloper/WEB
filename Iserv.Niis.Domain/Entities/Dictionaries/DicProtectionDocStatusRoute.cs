using Iserv.Niis.Domain.Entities.Dictionaries.DicMain;

namespace Iserv.Niis.Domain.Entities.Dictionaries
{
    /// <summary>
    /// Связь между статусом охранного документа и маршрутом.
    /// </summary>
    public class DicProtectionDocStatusRoute
    {
        /// <summary>
        /// ID статуса охранного документа.
        /// </summary>
        public int DicProtectionDocStatusId { get; set; }

        /// <summary>
        /// Статус охранного документа.
        /// </summary>
        public DicProtectionDocStatus DicProtectionDocStatus { get; set; }

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
