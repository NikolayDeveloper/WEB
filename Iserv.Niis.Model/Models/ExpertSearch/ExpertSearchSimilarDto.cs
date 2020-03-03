using Iserv.Niis.Domain.Helpers;
using System;

namespace Iserv.Niis.Model.Models.ExpertSearch
{
    public class ExpertSearchSimilarDto
    {
        public int Id { get; set; }
        public DateTimeOffset DateCreate { get; set; }
        public int RequestId { get; set; }
        public Owner.Type OwnerType { get; set; }
        public int? SimilarRequestId { get; set; }
        public Domain.Entities.Request.Request SimilarRequest { get; set; }
        public int? SimilarProtectionDocId { get; set; }
        public Domain.Entities.ProtectionDoc.ProtectionDoc SimilarProtectionDoc { get; set; }
        public int ImageSimilarity { get; set; }
        public int PhonSimilarity { get; set; }
        public int SemSimilarity { get; set; }
        public string ProtectionDocFormula { get; set; }
        public string ProtectionDocCategory { get; set; }
    }
}