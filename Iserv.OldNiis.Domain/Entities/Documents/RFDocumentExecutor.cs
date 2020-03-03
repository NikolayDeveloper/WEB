using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace NIIS.DBConverter.Entities.Documents
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    [Table("RF_DOCUMENT_EXECUTOR")]
    public class RFDocumentExecutor
    {
        [Column("U_ID")]
        public int Id { get; set; }
        [Column("DOCUMENT_ID")]
        public int DocumentId { get; set; }
        [Column("USER_ID")]
        public int UserId { get; set; }
    }
}
