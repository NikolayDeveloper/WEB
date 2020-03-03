using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// ReSharper disable InconsistentNaming

namespace NIIS.DBConverter.Entities.Documents
{
    [Table("DD_INFO")]
    public class DdInfo
    {
        [Column("U_ID")]
        [Key]
        public int U_ID { get; set; }
        [Column("date_create")]
        public DateTime? date_create { get; set; }
        [Column("stamp")]
        public DateTime? stamp { get; set; }
        [Column("FLAG_NINE")]
        public string FLAG_NINE { get; set; }
        [Column("FLAG_TTH")]
        public string FLAG_TTH { get; set; }
        [Column("FLAG_TTW")]
        public string FLAG_TTW { get; set; }
        [Column("FLAG_TPT")]
        public string FLAG_TPT { get; set; }
        [Column("FLAG_TAT")]
        public string FLAG_TAT { get; set; }
        [Column("FLAG_TN")]
        public string FLAG_TN { get; set; }
        [Column("COL_TZ")]
        public string COL_TZ { get; set; }
        [Column("PRIOR_TZ")]
        public string PRIOR_TZ { get; set; }
        [Column("AWARD_TZ")]
        public string AWARD_TZ { get; set; }
        [Column("FONT_TZ")]
        public string FONT_TZ { get; set; }
        [Column("TRANS_TZ")]
        public string TRANS_TZ { get; set; }
        [Column("INVERT_TZ")]
        public string INVERT_TZ { get; set; }
        [Column("D3_TZ")]
        public string D3_TZ { get; set; }
        [Column("COLOR_TZ")]
        public string COLOR_TZ { get; set; }
        [Column("REG_TZ")]
        public string REG_TZ { get; set; }
        [Column("TM_TRANSLIT")]
        public string TM_TRANSLIT { get; set; }
        [Column("TM_TRANSLATE")]
        public string TM_TRANSLATE { get; set; }
        [Column("TM_PRIORITET")]
        public string TM_PRIORITET { get; set; }
        [Column("SEL_NOMER")]
        public string SEL_NOMER { get; set; }
        [Column("SEL_ROOT")]
        public string SEL_ROOT { get; set; }
        [Column("SEL_FAMILY")]
        public string SEL_FAMILY { get; set; }
        [Column("PN_GOODS")]
        public string PN_GOODS { get; set; }
        [Column("PN_DSC")]
        public string PN_DSC { get; set; }
        [Column("PN_PLACE")]
        public string PN_PLACE { get; set; }
        [Column("SYS_EDS")]
        public string SYS_EDS { get; set; }
        [Column("EDS")]
        public byte[] EDS { get; set; }
        [Column("flBreedCountry")]
        public int? flBreedCountry { get; set; }
        [Column("flProductSpecialProp")]
        public string flProductSpecialProp { get; set; }
        [Column("flProductPalce")]
        public string flProductPalce { get; set; }
    }
}
