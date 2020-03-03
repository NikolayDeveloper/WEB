using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace NIIS.DBConverter.Entities.DBObjects {
    [Table("SP_TEMPLATE_TABLE")]
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class SPTemplateTable {
        [Column("U_ID")]
        public int Id { get; set; }

        [Column("TABLE_ID")]
        public int TableId { get; set; }

        [Column("COLUMN_ID")]
        public int? ColumnId { get; set; }

        [Column("TYPE_ID")]
        public int? TypeId { get; set; }

        [Column("NAME_ML_EN")]
        public string NameEn { get; set; }

        [Column("NAME_ML_RU")]
        public string NameRu { get; set; }

        [Column("NAME_ML_KZ")]
        public string NameKz { get; set; }

        [Column("CODE")]
        public string Code { get; set; }

        [Column("IS_REFERECE")]
        public string IsReference { get; set; }
    }
}
