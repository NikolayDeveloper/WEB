using System;
using Iserv.Niis.Domain.Abstract;
using Iserv.Niis.Domain.Entities.Dictionaries;
using Iserv.Niis.Domain.Entities.Dictionaries.Location;
using Iserv.Niis.Domain.EntitiesHistory.Patent;

namespace Iserv.Niis.Domain.Entities.ProtectionDoc
{
    public class ProtectionDocEarlyReg : Entity<int>, IHistorySupport, IHaveConcurrencyToken
    {
        public int ProtectionDocId { get; set; } //doc_id
        public ProtectionDoc ProtectionDoc { get; set; }
        public int EarlyRegTypeId { get; set; }
        public DicEarlyRegType EarlyRegType { get; set; }
        public int? PCTType { get; set; }
        public int? RegCountryId { get; set; }
        public DicCountry RegCountry { get; set; }
        public string RegNumber { get; set; }
        public DateTimeOffset? RegDate { get; set; }
        public string NameSD { get; set; }
        public string Description { get; set; }
        public DateTimeOffset? DateF1 { get; set; }
        public DateTimeOffset? DateF2 { get; set; }
        public DateTimeOffset? PriorityDate { get; set; }

        public Type GetHistoryEntity()
        {
            return typeof(PatentEarlyRegHistory);
        }
    }
}