using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
// ReSharper disable InconsistentNaming

namespace NIIS.DBConverter.Entities.References
{
    [Table("RF_IPC")]
    public class RfIPC
    {
        [Key]
        [Column("U_ID")]
        public int U_ID { get; set; }
        [Column("PATENT_ID")]
        public int PATENT_ID { get; set; }
        [Column("TYPE_ID")]
        public int TYPE_ID { get; set; }
        [Column("date_create")]
        public DateTime? date_create { get; set; }
        [Column("stamp")]
        public DateTime? stamp { get; set; }
        [Column("flIsMain")]
        public string flIsMain { get; set; }
    }
}
