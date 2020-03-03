using System;
using Iserv.Niis.Domain.Abstract;
using Iserv.Niis.Domain.Entities.Dictionaries;
using Iserv.Niis.Domain.Entities.Dictionaries.Location;
using Iserv.Niis.Domain.EntitiesHistory.Patent;

namespace Iserv.Niis.Domain.Entities.Request
{
    public class RequestEarlyReg : Entity<int>, IHistorySupport, IHaveConcurrencyToken
    {
        public int RequestId { get; set; } //doc_id
        public Request Request { get; set; }
        public int EarlyRegTypeId { get; set; }
        public DicEarlyRegType EarlyRegType { get; set; }
        public int? RegCountryId { get; set; }
        public DicCountry RegCountry { get; set; }
        public string RegNumber { get; set; }
        public DateTimeOffset? RegDate { get; set; }
        public DateTimeOffset? PriorityDate { get; set; }
        public string NameSD { get; set; }
        public string StageSD { get; set; }

        /// <summary>
        ///  Глава 1
        /// </summary>
        public string ChapterOne { get; set; }

        /// <summary>
        /// Дата главы 1
        /// </summary>
        public DateTimeOffset? DateOfChapterOne { get; set; }

        /// <summary>
        /// Глава 2
        /// </summary>
        public string ChapterTwo { get; set; }

        /// <summary>
        /// Дата главы 2
        /// </summary>
        public DateTimeOffset? DateOfChapterTwo { get; set; }

        /// <summary>
        /// Приоритетные данные Romarin
        /// </summary>
        public string ITMRawPriorityData { get; set; }

        public Type GetHistoryEntity()
        {
            return typeof(PatentEarlyRegHistory);
        }
    }
}