using System.Collections.Generic;
using Iserv.Niis.Domain.Abstract;

namespace Iserv.Niis.Domain.Entities.Dictionaries
{
    /// <summary>
    /// Статус заявки.
    /// </summary>
    public class DicRequestStatus : DictionaryEntity<int>, IHaveConcurrencyToken
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        public DicRequestStatus()
        {
            DicRequestStatusesRoutes = new HashSet<DicRequestStatusRoute>();
        }

        /// <summary>
        /// Связи между статусами заявок и маршрутами.
        /// </summary>
        public ICollection<DicRequestStatusRoute> DicRequestStatusesRoutes { get; set; }
    }
}