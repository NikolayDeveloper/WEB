using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// ReSharper disable InconsistentNaming

namespace NIIS.DBConverter.Entities.References
{
    [Table("RF_TM_ICGS")]
    public class RfTmIcgs
    {
        [Column("U_ID")]
        [Key]
        public int U_ID { get; set; }
        [Column("DOC_ID")]
        public int? DOC_ID { get; set; }
        [Column("ICPS_ID")]
        public int? ICPS_ID { get; set; }
        [Column("IS_NEGATIVE")]
        public string IS_NEGATIVE { get; set; }
        [Column("DSC")]
        public string DSC { get; set; }
        [Column("DSC_KZ")]
        public string DSC_KZ { get; set; }
        [Column("NDSC_CLOB")]
        public string NDSC_CLOB { get; set; }
        [Column("DOCUMENT_ID")]
        public int? DOCUMENT_ID { get; set; }
        [Column("date_create")]
        public DateTime? date_create { get; set; }
        [Column("stamp")]
        public DateTime? stamp { get; set; }
        [Column("flIsNegativePartial")]
        public string flIsNegativePartial { get; set; }
        [Column("flDscStarted")]
        public string flDscStarted { get; set; }
    }
}
