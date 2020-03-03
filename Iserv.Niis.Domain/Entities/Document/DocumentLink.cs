using Iserv.Niis.Domain.Abstract;

namespace Iserv.Niis.Domain.Entities.Document
{
    /// <summary>
    /// Сcылка на документ
    /// </summary>
    public class DocumentLink : Entity<int>
    {
        /// <summary>
        /// Родитель
        /// </summary>
        public int ParentDocumentId { get; set; }
        public Document ParentDocument { get; set; }

        /// <summary>
        /// Дочерний
        /// </summary>
        public int ChildDocumentId { get; set; }
        public Document ChildDocument { get; set; }
    }
}
