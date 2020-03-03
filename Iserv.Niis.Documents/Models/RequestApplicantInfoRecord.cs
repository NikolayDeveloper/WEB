using System;

namespace Iserv.Niis.Documents.Models
{
    public class RequestApplicantInfoRecord
    {
        public string RequestNum { get; set; }
        public string DeclarantShortInfo { get; set; }
        public string PatentAttorneyShortInfo { get; set; }
        public string ConfidantShortInfo { get; set; }
        public string RequestDate { get; set; }
        public string PatentName { get; set; }
        public int RequestCount { get; set; }
    }
}