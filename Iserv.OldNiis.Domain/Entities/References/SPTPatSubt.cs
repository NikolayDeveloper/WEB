using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace NIIS.DBConverter.Entities.References {
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    [Table("SPT_PAT_SUBT")]
    public class SPTPatSubt {
        [Column("U_ID")]
        public int Id { get; set; }

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

        [Column("S1")]
        public string S1 { get; set; }

        [Column("S2")]
        public string S2 { get; set; }

        [Column("S1KZ")]
        public string S1Kz { get; set; }

        [Column("S2KZ")]
        public string S2Kz { get; set; }
    }
}
