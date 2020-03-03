using System;

namespace Iserv.Niis.ExternalServices.Domain.Entities
{
    public class IntegrationLogDigitalSignature
    {
        public int Id { get; set; }
        public bool SignatureIsValid { get; set; }
        public bool CertificateIsCorrect { get; set; }
        public string CheckNote { get; set; }
        public byte[] RawData { get; set; }
        public DateTimeOffset? PeriodFrom { get; set; }
        public DateTimeOffset? PeriodTo { get; set; }
        public string EMail { get; set; }
        public string Iin { get; set; }
        public string Person { get; set; }
        public string Bin { get; set; }
        public string Company { get; set; }
    }
}