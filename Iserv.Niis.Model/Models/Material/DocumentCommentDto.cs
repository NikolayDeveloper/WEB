using System;

namespace Iserv.Niis.Model.Models.Material
{
    /// <summary>
    /// Комментарии к документу
    /// </summary>
    public class DocumentCommentDto
    {
        /// <summary>
        /// Уникальный идентификатор
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Документу
        /// </summary>
        public int DocumentId { get; set; }

        /// <summary>
        /// Этап на котором был оставлен комметарий
        /// </summary>
        public int? WorkflowId { get; set; }

        /// <summary>
        /// Автор комментария
        /// </summary>
        public int AuthorId { get; set; }
        public string AuthorInitials { get; set; }

        /// <summary>
        /// Комментарий
        /// </summary>
        public string Comment { get; set; }

        /// <summary>
        /// Дата создания комметария
        /// </summary>
        public DateTimeOffset DateCreate { get; set; }

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
