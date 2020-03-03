using System.Collections.Generic;
using Iserv.Niis.Domain.Abstract;

namespace Iserv.Niis.Domain.Entities.Dictionaries.DicMain
{
    /// <summary>
    /// Маршрут.
    /// </summary>
    public class DicRoute : DictionaryEntity<int>, IReference, IHaveConcurrencyToken
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        public DicRoute()
        {
            DicRequestStatusesRoutes = new HashSet<DicRequestStatusRoute>();
            DicProtectionDocStatusesRoutes = new HashSet<DicProtectionDocStatusRoute>();
        }

        /// <summary>
        /// Связи между статусами заявок и маршрутами.
        /// </summary>
        public ICollection<DicRequestStatusRoute> DicRequestStatusesRoutes { get; set; }

        /// <summary>
        /// Связи между статусами охранных документов и маршрутами.
        /// </summary>
        public ICollection<DicProtectionDocStatusRoute> DicProtectionDocStatusesRoutes { get; set; }

        /// <summary>
        /// Этапы маршрутов.
        /// </summary>
        public ICollection<DicRouteStage> RouteStages { get; set; }

        // DocumentTypes
        // DocumentWorkflows

        /// <summary>
        /// Возвращает строку представляющую наименование маршрута.
        /// </summary>
        /// <returns>Наименование маршрута.</returns>
        public override string ToString()
        {
            return NameRu;
        }


        public static class Codes
        {
            public const string OUT = "OUT";
            public const string IN = "IN";
            public const string W = "W";
            public const string DR = "DR";
            public const string TM = "TM";
            public const string SA = "SA";
            public const string S2 = "S2";
            public const string U = "U";
            public const string GR = "GR";
            public const string NMPT = "NMPT";
            public const string TMI = "TMI";
            public const string B = "B";
            public const string DK = "DK";
        }
    }
}