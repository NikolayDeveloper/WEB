using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// ReSharper disable InconsistentNaming

namespace NIIS.DBConverter.Entities.Patent
{
    [Table("DD_EARLYREG")]
    public class DdEarlyReg
    {
        [Column("U_ID")]
        [Key]
        public int U_ID { get; set; }
        [Column("date_create")]
        public DateTime? date_create { get; set; }
        [Column("stamp")]
        public DateTime? stamp { get; set; }
        [Column("DOCUMENT_ID")]
        public int DOCUMENT_ID { get; set; }
        [Column("TYPE_ID")]
        public int TYPE_ID { get; set; }
        [Column("COUNTRY_ID")]
        public int? COUNTRY_ID { get; set; }
        [Column("REQ_NUM")]
        public string REQ_NUM { get; set; }
        [Column("REQ_DATE")]
        [DataType(DataType.Date)]
        public DateTime REQ_DATE { get; set; }
        [Column("REQ_PRIOR")]
        [DataType(DataType.Date)]
        public DateTime? REQ_PRIOR { get; set; }
        [Column("SA_NAME")]
        public string SA_NAME { get; set; }
        [Column("SA_STAGE")]
        public string SA_STAGE { get; set; }
    }
}
