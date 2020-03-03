using System;

namespace Iserv.Niis.Model.Models.EarlyReg
{
    public class RequestEarlyRegDto : IEquatable<RequestEarlyRegDto>
    {
        public int? Id { get; set; }
        public int? EarlyRegTypeId { get; set; }
        public int? RegCountryId { get; set; }
        public string CountryNameRu { get; set; }
        public string RegNumber { get; set; }
        public DateTimeOffset? RegDate { get; set; }
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

        public bool Equals(RequestEarlyRegDto other)
        {
            if (other == null)
            {
                return false;
            }

            return Id.Equals(other.Id) &&
                   EarlyRegTypeId.Equals(other.EarlyRegTypeId) &&
                   RegCountryId.Equals(other.RegCountryId) &&
                   string.Equals(RegNumber, other.RegNumber) &&
                   Equals(RegDate, other.RegDate);
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            return Equals(obj as RequestEarlyRegDto);
        }
    }
}