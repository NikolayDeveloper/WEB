using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// ReSharper disable InconsistentNaming

namespace NIIS.DBConverter.Entities.References
{
    [Table("tbReferences")]
    public class TbReferences
    {
        [Column("flId")]
        [Key]
        public int flId { get; set; }
        [Column("flRu")]
        public string flRu { get; set; }
        [Column("flKz")]
        public string flKz { get; set; }
        [Column("flType")]
        public string flType { get; set; }
        [Column("flParentId")]
        public int? flParentId { get; set; }
    }
}
