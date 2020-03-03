using System;
using Iserv.Niis.Domain.Abstract;
using Iserv.Niis.Domain.Entities.Dictionaries;
using Iserv.Niis.Domain.Entities.Dictionaries.Location;

namespace Iserv.Niis.Domain.Entities.Document
{
    public class DocumentEarlyReg : Entity<int>, IHaveConcurrencyToken
    {
        public int DocumentId { get; set; }
        public Document Document { get; set; }
        public int EarlyRegTypeId { get; set; }
        public DicEarlyRegType EarlyRegType { get; set; }
        public int? CountryId { get; set; }
        public DicCountry Country { get; set; }
        public string RegNumber { get; set; }
        public DateTimeOffset? RegDate { get; set; }
        public DateTimeOffset? PriorityDate { get; set; }
        public string NameSD { get; set; }
        public string StageSD { get; set; }
    }
}