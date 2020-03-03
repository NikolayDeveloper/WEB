using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// ReSharper disable InconsistentNaming

namespace NIIS.DBConverter.Entities.Others
{
    [Table("RF_STAGE_DOCUMENT")]
    public class RfStageDocument
    {
        [Column("U_ID")]
        [Key]
        public int U_ID { get; set; }
        [Column("STAGE_ID")]
        public int STAGE_ID { get; set; }
        [Column("DOCTYPE_ID")]
        public int? DOCTYPE_ID { get; set; }
        [Column("DOCLASS_ID")]
        public int? DOCLASS_ID { get; set; }
    }
}
