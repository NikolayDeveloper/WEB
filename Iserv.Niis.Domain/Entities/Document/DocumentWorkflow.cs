using Iserv.Niis.Domain.Entities.Workflow;
using System.Collections.Generic;

namespace Iserv.Niis.Domain.Entities.Document
{
    public class DocumentWorkflow : BaseWorkflow
    {
        public DocumentWorkflow()
        {
            DocumentWorkflowViewers = new HashSet<DocumentWorkflowViewer>();
        }

        public bool IsCurent { get; set; }

        public int OwnerId { get; set; }
        public Document Owner { get; set; }
        public DocumentUserSignature DocumentUserSignature { get; set; }

        /// <summary>
        /// Список сотрудников с доступом на просмотр документа
        /// </summary>
        public ICollection<DocumentWorkflowViewer> DocumentWorkflowViewers { get; set; }
    }
}