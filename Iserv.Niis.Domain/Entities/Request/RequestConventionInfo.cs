using System;
using Iserv.Niis.Domain.Abstract;
using Iserv.Niis.Domain.Entities.Dictionaries;
using Iserv.Niis.Domain.Entities.Dictionaries.Location;

namespace Iserv.Niis.Domain.Entities.Request
{
    public class RequestConventionInfo : Entity<int>, IHaveConcurrencyToken
    {
        public int RequestId { get; set; }
        public Request Request { get; set; }
        public int? CountryId { get; set; }
        public DicCountry Country { get; set; }
        public int? EarlyRegTypeId { get; set; }
        public DicEarlyRegType EarlyRegType { get; set; }
        public DateTimeOffset? InternationalAppToNationalPhaseTransferDate { get; set; }
        public DateTimeOffset? DateInternationalApp { get; set; }
        public string RegNumberInternationalApp { get; set; }
        public DateTimeOffset? PublishDateInternationalApp { get; set; }
        public string PublishRegNumberInternationalApp { get; set; }
        public DateTimeOffset? DateEurasianApp { get; set; }
        public string RegNumberEurasianApp { get; set; }
        public DateTimeOffset? PublishDateEurasianApp { get; set; }
        public string PublishRegNumberEurasianApp { get; set; }
        public string HeadIps { get; set; }
        public DateTimeOffset? TermNationalPhaseFirsChapter { get; set; }
        public DateTimeOffset? TermNationalPhaseSecondChapter { get; set; }
    }
}