using System;

namespace Iserv.Niis.Model.Models.BibliographicData
{
    public class IcgsDto : IEquatable<IcgsDto>
    {
        public int? Id { get; set; }
        public int IcgsId { get; set; }
        public string IcgsName { get; set; }
        public string ClaimedDescription { get; set; }
        public string ClaimedDescriptionEn { get; set; }
        public string Description { get; set; }
        public string DescriptionKz { get; set; }
        public string DescriptionNew { get; set; }
        public string NegativeDescription { get; set; }
        public bool? IsRefused { get; set; }
        public bool? IsPartialRefused { get; set; }
        public bool? IsSplit { get; set; }
        public string ReasonForPartialRefused { get; set; }

        public bool Equals(IcgsDto other)
        {
            if (other == null)
            {
                return false;
            }
            return Id.Equals(other.Id) &&
                   IcgsId.Equals(other.IcgsId) &&
                   string.Equals((string)ClaimedDescription, (string)other.ClaimedDescription) &&
                   string.Equals((string)ClaimedDescriptionEn, (string)other.ClaimedDescriptionEn) &&
                   string.Equals((string)Description, (string)other.Description) &&
                   string.Equals((string)DescriptionKz, (string)other.DescriptionKz) &&
                   string.Equals((string)NegativeDescription, (string)other.NegativeDescription) &&
                   IsRefused.Equals(other.IsRefused) &&
                   IsPartialRefused.Equals(other.IsPartialRefused) &&
                   string.Equals((string)ReasonForPartialRefused, (string)other.ReasonForPartialRefused);
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }
            return Equals(obj as IcgsDto);
        }
    }
}