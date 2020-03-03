using System;

namespace Iserv.Niis.Model.Models.BibliographicData
{
    public class ConventionInfoDto : IEquatable<ConventionInfoDto>
    {
        public int? Id { get; set; }
        public int RequestId { get; set; }
        public int? CountryId { get; set; }
        public int? EarlyRegTypeId { get; set; }
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

        public bool Equals(ConventionInfoDto other)
        {
            if (other == null)
            {
                return false;
            }

            return Id.Equals(other.Id) &&
                   RequestId.Equals(other.RequestId) &&
                   CountryId.Equals(other.CountryId) &&
                   EarlyRegTypeId.Equals(other.EarlyRegTypeId) &&
                   string.Equals((string) RegNumberInternationalApp, (string) other.RegNumberInternationalApp) &&
                   string.Equals((string) PublishRegNumberInternationalApp,
                       (string) other.PublishRegNumberInternationalApp) &&
                   string.Equals((string) RegNumberEurasianApp, (string) other.RegNumberEurasianApp) &&
                   string.Equals((string) PublishRegNumberEurasianApp, (string) other.PublishRegNumberEurasianApp) &&
                   string.Equals((string) HeadIps, (string) other.HeadIps) &&
                   Object.Equals(DateInternationalApp, other.DateInternationalApp) &&
                   Object.Equals(PublishDateInternationalApp, other.PublishDateInternationalApp) &&
                   Object.Equals(DateEurasianApp, other.DateEurasianApp) &&
                   Object.Equals(PublishDateEurasianApp, other.PublishDateEurasianApp) &&
                   Object.Equals(InternationalAppToNationalPhaseTransferDate,
                       other.InternationalAppToNationalPhaseTransferDate) &&
                   Object.Equals(TermNationalPhaseFirsChapter, other.TermNationalPhaseFirsChapter) &&
                   Object.Equals(TermNationalPhaseSecondChapter, other.TermNationalPhaseSecondChapter);
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            return Equals(obj as ConventionInfoDto);
        }
    }
}