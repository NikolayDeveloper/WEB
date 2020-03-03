using Iserv.Niis.Domain.Abstract;
using Iserv.Niis.Domain.Entities.Security;
using System;

namespace Iserv.Niis.Domain.Entities.Document
{
    /// <summary>
    /// Комментарии к документу
    /// </summary>
    public class DocumentComment : Entity<int>, ISoftDeletable
    {
        /// <summary>
        /// Документу
        /// </summary>
        public int DocumentId { get; set; }
        public Document Document { get; set; }

        /// <summary>
        /// Этап на котором был оставлен комметарий
        /// </summary>
        public int? WorkflowId { get; set; }
        public DocumentWorkflow Workflow { get; set; }

        /// <summary>
        /// Автор комментария
        /// </summary>
        public int AuthorId { get; set; }
        public ApplicationUser Author { get; set; }

        /// <summary>
        /// Комментарий
        /// </summary>
        public string Comment { get; set; }

        #region ISoftDeletable

        /// <summary>
        /// Флаг удаления файла
        /// </summary>
        public bool IsDeleted { get; set; }

        /// <summary>
        /// Дата удаления файла
        /// </summary>
        public DateTimeOffset? DeletedDate { get; set; }

        #endregion
    }
}
