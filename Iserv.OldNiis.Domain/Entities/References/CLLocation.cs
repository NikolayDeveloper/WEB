
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace NIIS.DBConverter.Entities.References
{
    [Table("CL_LOCATION")]
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class CLLocation
    {
        [Column("U_ID")]
        public int Id { get; set; }
        [Column("TYPE_ID")]
        public int TypeId { get; set; }
        [Column("CODE")]
        public string Code { get; set; }
        [Column("NAME_ML_EN")]
        public string NameEn { get; set; }
        [Column("NAME_ML_RU")]
        public string NameRu { get; set; }
        [Column("NAME_ML_KZ")]
        public string NameKz { get; set; }
        [Column("F_ID")]
        public int? ParentId { get; set; }
        [Column("flOrder")]
        public int? OrderId { get; set; }
        [Column("flStatId")]
        public string StatId { get; set; }
        [Column("flStatParentId")]
        public string StatParentId { get; set; }
    }
}
