
using NIIS.DBConverter.Entities.Documents;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace NIIS.DBConverter.Entities.References {
    [Table("RF_MESSAGE_DOCUMENT")]
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class RFMessageDocument {
        [Column("U_ID")]
        public int Id { get; set; }
        [Column("date_create")]
        public DateTime? DateCreate { get; set; }
        [ForeignKey("DDDocument")]
        [Column("DOCUMENT_ID")]
        public int?  DocumentId { get; set; }
        public DDDocument DDDocument { get;set;}
        [ForeignKey("RefDDDocument")]
        [Column("REFDOCUMENT_ID")]
        public int RefDocumentId { get; set; }
        public DDDocument RefDDDocument { get; set; }
        [Column("IS_ANSWER")]
        public string IsAnswer { get; set; }
    }
}
