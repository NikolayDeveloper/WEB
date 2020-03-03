using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// ReSharper disable InconsistentNaming

namespace NIIS.DBConverter.Entities.Others
{
    [Table("CL_TM_ICCOLOR")]
    public class ClTmIColor
    {
        [Column("U_ID")]
        [Key]
        public int U_ID { get; set; }
        [Column("CODE")]
        public string CODE { get; set; }
        [Column("NAME_ML_EN")]
        public string NAME_ML_EN { get; set; }
        [Column("NAME_ML_RU")]
        public string NAME_ML_RU { get; set; }
        [Column("NAME_ML_KZ")]
        public string NAME_ML_KZ { get; set; }
    }
}
