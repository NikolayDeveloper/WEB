using System;

namespace Iserv.Niis.Model.Models.Material
{
    public class DocumentUserSignatureDto
    {
        public int Id { get; set; }
        public int WorkflowId { get; set; }
        public int UserId { get; set; }
        public DateTimeOffset SignatureDate { get; set; }
        public string PlainData { get; set; }
        public string SignedData { get; set; }
        public string SignerCertificate { get; set; }
        public bool IsValidCertificate { get; set; }
        public string SignatureError { get; set; }
        public string Password { get; set; }
        public string CertStoragePath { get; set; }
    }
}