using System.ComponentModel.DataAnnotations.Schema;

namespace NIIS.DBConverter.Entities.References
{
    [Table("CL_TEMPLATE")]
    public class CLTemplate
    {
        [Column("U_ID")]
        public int Id { get; set; }
        
        [Column("F_ID")]
        public int? ParentId { get; set; }
        
        [Column("CODE")]
        public string Code { get; set; }

        [Column("NAME_ML_EN")]
        public string NameEn { get; set; }

        [Column("NAME_ML_RU")]
        public string NameRu { get; set; }
        
        [Column("NAME_ML_KZ")]
        public string NameKz { get; set; }
        
        [Column("DESCRIPTION")]
        public string Description { get; set; }
    }
}
