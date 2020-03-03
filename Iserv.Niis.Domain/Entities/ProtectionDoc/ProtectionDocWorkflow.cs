using Iserv.Niis.Domain.Entities.Security;
using Iserv.Niis.Domain.Entities.Workflow;

namespace Iserv.Niis.Domain.Entities.ProtectionDoc
{
    public class ProtectionDocWorkflow : BaseWorkflow
    {
        public int OwnerId { get; set; }
        public Entities.ProtectionDoc.ProtectionDoc Owner { get; set; }
        public int? SecondaryCurrentUserId { get; set; }
        public ApplicationUser SecondaryCurrentUser { get; set; }
    }
}