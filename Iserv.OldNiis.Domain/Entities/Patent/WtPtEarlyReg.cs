using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// ReSharper disable InconsistentNaming

namespace NIIS.DBConverter.Entities.Patent
{
    [Table("WT_PT_EARLYREG")]
    public class WtPtEarlyReg
    {
        [Column("U_ID")]
        [Key]
        public int U_ID { get; set; }
        [Column("date_create")]
        public DateTime? date_create { get; set; }
        [Column("DOC_ID")]
        public int? DOC_ID { get; set; }
        [Column("ETYPE_ID")]
        public int? ETYPE_ID { get; set; }
        [Column("OLD_STRUCT")]
        public string OLD_STRUCT { get; set; }
        [Column("REQ_NUMBER")]
        public string REQ_NUMBER { get; set; }
        [Column("REQ_COUNTRY")]
        public int? REQ_COUNTRY { get; set; }
        [Column("REQ_DATE")]
        [DataType(DataType.Date)]
        public DateTime? REQ_DATE { get; set; }
        [Column("SA_NAME")]
        public string SA_NAME { get; set; }
        [Column("DESCRIPTION")]
        public string DESCRIPTION { get; set; }
        [Column("PCTYPE")]
        public int? PCTYPE { get; set; }
        [Column("DATE_F1")]
        [DataType(DataType.Date)]
        public DateTime? DATE_F1 { get; set; }
        [Column("DATE_F2")]
        [DataType(DataType.Date)]
        public DateTime? DATE_F2 { get; set; }
    }
}
