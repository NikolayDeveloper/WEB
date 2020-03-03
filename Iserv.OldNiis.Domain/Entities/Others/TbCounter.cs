using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// ReSharper disable InconsistentNaming

namespace NIIS.DBConverter.Entities.Others
{
    [Table("tbCounter")]
    public class TbCounter
    {
        [Column("flId")]
        [Key]
        public int flId { get; set; }
        [Column("flCode")]
        public string flCode { get; set; }
        [Column("flCount")]
        public int flCount { get; set; }
        [Column("flLut")]
        public DateTime? flLut { get; set; }
    }
}
