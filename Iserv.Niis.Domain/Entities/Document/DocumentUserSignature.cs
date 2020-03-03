using Iserv.Niis.Domain.Abstract;
using Iserv.Niis.Domain.Entities.Security;

namespace Iserv.Niis.Domain.Entities.Document
{
    public class DocumentUserSignature : Entity<int>, IHaveConcurrencyToken
    {
        public int WorkflowId { get; set; }
        public DocumentWorkflow Workflow { get; set; }
        public int UserId { get; set; }
        public ApplicationUser User { get; set; }
        public string PlainData { get; set; }
        public string SignedData { get; set; }
        public string SignerCertificate { get; set; }
        public bool IsValidCertificate { get; set; }
        public string SignatureError { get; set; }
    }
}