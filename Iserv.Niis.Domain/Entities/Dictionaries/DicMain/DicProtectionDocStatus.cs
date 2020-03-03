using System.Collections.Generic;
using Iserv.Niis.Domain.Abstract;

namespace Iserv.Niis.Domain.Entities.Dictionaries.DicMain
{
    /// <summary>
    /// Статус охранного документа.
    /// </summary>
    public class DicProtectionDocStatus : DictionaryEntity<int>, IReference, IHaveConcurrencyToken
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        public DicProtectionDocStatus()
        {
            DicProtectionDocStatusesRoutes = new HashSet<DicProtectionDocStatusRoute>();
        }

        /// <summary>
        /// Связи между статусами охранных документов и маршрутами.
        /// </summary>
        public ICollection<DicProtectionDocStatusRoute> DicProtectionDocStatusesRoutes { get; set; }
    }
}