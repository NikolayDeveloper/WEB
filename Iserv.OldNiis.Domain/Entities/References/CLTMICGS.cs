
using System.ComponentModel.DataAnnotations.Schema;

namespace NIIS.DBConverter.Entities.References
{
    [Table("CL_TM_ICGS")]
    public class CLTMICGS
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

        [Column("FULL_DESC")]
        public string FullDesc { get; set; }

        [Column("REDACTION_NUM")]
        public int RevisionId { get; set; }

        [Column("DOC_ID")]
        public int? DocId { get; set; }

        [Column("flDescShort")]
        public string ShortDesc { get; set; }
    }
}
