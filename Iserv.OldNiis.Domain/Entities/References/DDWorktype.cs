using System.ComponentModel.DataAnnotations.Schema;

namespace NIIS.DBConverter.Entities.References
{
    [Table("DD_WORKTYPE")]
    // ReSharper disable once InconsistentNaming
    public class DDWorktype
    {
        [Column("U_ID")]
        public int Id { get; set; }

        [Column("NAME_ML_RU")]
        public string NameRu { get; set; }
        
        [Column("NAME_ML_KZ")]
        public string NameKz { get; set; }
        
        [Column("NAME_ML_EN")]
        public string NameEn { get; set; }
        
        [Column("CODE")]
        public string Code { get; set; }
        
        [Column("DESCRIPT")]
        public string Description { get; set; }
    }
}
