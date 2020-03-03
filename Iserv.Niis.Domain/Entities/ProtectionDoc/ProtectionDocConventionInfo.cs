using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Iserv.Niis.Domain.Abstract;
using Iserv.Niis.Domain.Entities.Dictionaries;
using Iserv.Niis.Domain.Entities.Dictionaries.Location;

namespace Iserv.Niis.Domain.Entities.ProtectionDoc
{
    public class ProtectionDocConventionInfo : Entity<int>, IHaveConcurrencyToken
    {
        public int ProtectionDocId { get; set; }
        public ProtectionDoc ProtectionDoc { get; set; }
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
