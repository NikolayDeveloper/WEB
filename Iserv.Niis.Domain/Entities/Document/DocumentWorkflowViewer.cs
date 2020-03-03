using Iserv.Niis.Domain.Abstract;
using Iserv.Niis.Domain.Entities.Security;

namespace Iserv.Niis.Domain.Entities.Document
{
    /// <summary>
    /// Список сотрудников с доступом на просмотр этапа
    /// </summary>
    public class DocumentWorkflowViewer : Entity<int>, IHaveConcurrencyToken
    {
        /// <summary>
        /// Этап
        /// </summary>
        public int DocumentWorkflowId { get; set; }
        public DocumentWorkflow DocumentWorkflow { get; set; }

        /// <summary>
        /// Должность сотрудника
        /// </summary>
        public int UserId { get; set; }
        public ApplicationUser User { get; set; }
    }
}
