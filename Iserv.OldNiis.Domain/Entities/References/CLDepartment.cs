
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace NIIS.DBConverter.Entities.References
{
    [Table("CL_DEPARTMENT")]
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class CLDepartment
    {
        [Column("U_ID")]
        public int Id { get; set; }
        [Column("F_ID")]
        public int? ParentId { get; set; }
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
        [Column("IS_MONITORING")]
        public string IsMonitoring { get; set; }
        [Column("flTNameRu")]
        public string TNameRu { get; set; }
    }
}
