using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace NIIS.DBConverter.Entities.DBObjects {
    [Table("ST_COLUMN_KIND")]
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class STColumnKind {
        [Column("U_ID")]
        public int Id { get; set; }
        [Column("CODE")]
        public string Code { get; set; }
        [Column("NAME_ML_KZ")]
        public string NameKz { get; set; }
        [Column("NAME_ML_EN")]
        public string NameEn { get; set; }
        [Column("NAME_ML_RU")]
        public string NameRu { get; set; }
    }
}
