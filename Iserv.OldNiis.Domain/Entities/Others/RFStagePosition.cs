using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace NIIS.DBConverter.Entities.Others
{
    [Table("RF_STAGE_POSITION")]
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class RFStagePosition {
        [Column("U_ID")]
        public int Id { get; set; }
        [Column("STAGE_ID")]
        public int StageId { get; set; }
        [Column("USER_ID")]
        public int? UserId { get; set; }
        [Column("GROUP_ID")]
        public int? GroupId { get; set; }
    }
}
