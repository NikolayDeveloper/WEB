using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// ReSharper disable InconsistentNaming

namespace NIIS.DBConverter.Entities.Documents {
    [Table("DD_DOCUMENT_DATA")]
    public class DdDocumentData {
        [Column("U_ID")]
        [Key]
        public decimal U_ID { get; set; }

        [Column("SYS_TEMPLATE_DATA")]
        public string SYS_TEMPLATE_DATA { get; set; }

        [Column("TEMPLATE_DATA")]
        public byte[] TEMPLATE_DATA { get; set; }

        [Column("SYS_ANY_DATA")]
        public string SYS_ANY_DATA { get; set; }

        [Column("ANY_DATA")]
        public byte[] ANY_DATA { get; set; }

        [Column("DOCUMENT_ID")]
        public int? DOCUMENT_ID { get; set; }

        [Column("date_create")]
        public DateTime? date_create { get; set; }

        [Column("stamp")]
        public DateTime? stamp { get; set; }

        [Column("flFingerPrintTempl")]
        public string flFingerPrintTempl { get; set; }
    }
}
