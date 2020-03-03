using Iserv.Niis.Domain.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iserv.Niis.Domain.Entities.ProtectionDoc
{
    public class ProtectionDocParallelWorkflow
    {
        public int Id { get; set; }
        public int ProtectionDocWorkflowId { get; set; }
        public bool IsFinished { get; set; }
        public int OwnerId { get; set; }
    }
}
