using System.ComponentModel.DataAnnotations.Schema;

namespace NIIS.DBConverter.Entities.Others {
    [Table("RF_STAGE_NEXT")]
    // ReSharper disable once InconsistentNaming
    public class RFStageNext {
        [Column("U_ID")]
        public int Id { get; set; }
        [Column("STAGE_ID")]
        public int StageId { get; set; }
        [Column("NEXTSTAGE_ID")]
        public int NextStageId { get; set; }
        [Column("IS_AUTOMATIC")]
        public string IsAutomatic { get; set; }
        [Column("IS_PARALLEL")]
        public string IsParallel { get; set; }
        [Column("IS_RETURN")]
        public string IsReturn { get; set; }
    }
}
