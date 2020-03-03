using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace NIIS.DBConverter.Entities.DBObjects
{
    [Table("ST_COLUMN_TYPE")]
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class STColumnType
    {
        [Column("U_ID")]
        public int Id { get; set; }
        [Column("CODE")]
        public string Code { get; set; }
        [Column("NAMEML_KZ")]
        public string NameKz { get; set; }
        [Column("NAMEML_EN")]
        public string NameEn { get; set; }
        [Column("NAMEML_RU")]
        public string NameRu { get; set; }
        [Column("DESCRIPTION")]
        public string Desc { get; set; }
    }
}
