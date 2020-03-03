using System.ComponentModel.DataAnnotations.Schema;

namespace NIIS.DBConverter.Entities.References
{
    [Table("CL_STAGE")]
    public class CLStage
    {
        [Column("U_ID")]
        public int Id { get; set; }
        [Column("CODE")]
        public string Code { get; set; }
        [Column("NAME_ML_EN")]
        public string NameEn { get; set; }
        [Column("NAME_ML_RU")]
        public string NameRu { get; set; }
        [Column("NAME_ML_KZ")]
        public string NameKz { get; set; }
        [Column("DESCRIPT")]
        public string Description { get; set; }
        [Column("IS_SYSTEM")]
        public string IsSystem { get; set; }
        [Column("interval")]
        public int? Interval { get; set; }
        [Column("WORKTYPE_ID")]
        public int? WorktypeId { get; set; }
        [Column("IS_FIRST")]
        public string IsFirst { get; set; }
        [Column("IS_LAST")]
        public string IsLast { get; set; }
        [Column("IS_MULTIUSER")]
        public string IsMultyUser { get; set; }
        [Column("IS_RETURNING")]
        public string IsReturning { get; set; }
        [Column("flOnlineStatus")]
        public int? OnlineStatusId { get; set; }
        [Column("flIntervalDays")]
        public int? IntervalDays { get; set; }
    }
}
