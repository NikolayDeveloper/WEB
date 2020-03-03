
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace NIIS.DBConverter.Entities.References
{
    [Table("tbAvailabilityCorrespondence")]
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class tbAvailabilityCorrespondence
    {
        [Column("U_ID")]
        public int Id { get; set; }
        [Column("flPatentType")]
        public int PatentType { get; set; }
        [Column("flStageId")]
        public int StageId { get; set; }
        [Column("flCorId")]
        public int CorId { get; set; }
        [Column("flStatus")]
        public int Status { get; set; }
    }
}
