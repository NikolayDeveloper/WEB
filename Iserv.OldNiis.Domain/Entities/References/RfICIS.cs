using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// ReSharper disable InconsistentNaming

namespace NIIS.DBConverter.Entities.References
{
    [Table("RF_ICIS")]
    public class RfICIS
    {
        [Column("U_ID")]
        [Key]
        public int U_ID { get; set; }
        [Column("date_create")]
        public DateTime? date_create { get; set; }
        [Column("PATENT_ID")]
        public int PATENT_ID { get; set; }
        [Column("TYPE_ID")]
        public int TYPE_ID { get; set; }
        [Column("OLD_STRUCT")]
        public string OLD_STRUCT { get; set; }
    }
}
