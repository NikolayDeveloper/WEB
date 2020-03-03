using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// ReSharper disable InconsistentNaming

namespace NIIS.DBConverter.Entities.Patent
{
    [Table("WT_PT_REDEFINE")]
    public class WtPtRedefine
    {
        [Column("U_ID")]
        [Key]
        public int U_ID { get; set; }
        [Column("date_create")]
        public DateTime? date_create { get; set; }
        [Column("DOC_ID")]
        public int? DOC_ID { get; set; }
        [Column("REDEF_DATE")]
        [DataType(DataType.Date)]
        public DateTime? REDEF_DATE { get; set; }
        [Column("TYPE_ID")]
        public int? TYPE_ID { get; set; }
        [Column("DSC")]
        public string DSC { get; set; }
        [Column("DSC_KZ")]
        public string DSC_KZ { get; set; }
    }
}
