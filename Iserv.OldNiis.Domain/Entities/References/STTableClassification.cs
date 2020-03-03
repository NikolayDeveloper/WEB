using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace NIIS.DBConverter.Entities.References
{
    [Table("ST_TABLE_CLASSIFICATION")]
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class STTableClassification
    {
        [Column("U_ID")]
        public int Id { get; set; }

        [Column("F_ID")]
        public int? ParentId { get; set; }

        [Column("CODE")]
        public string Code { get; set; }

        [Column("NAMEML_RU")]
        public string NameRu { get; set; }

        [Column("NAMEML_EN")]
        public string NameEn { get; set; }

        [Column("NAMEML_KZ")]
        public string NameKz { get; set; }

        [Column("DESCRIPTION")]
        public string Description { get; set; }

        [Column("IMAGE")]
        public byte[] Image { get; set; }

        [Column("SYS_IMAGE")]
        public string ImageName { get; set; }

        [Column("stamp")]
        public DateTime? Stamp { get; set; }
    }
}
