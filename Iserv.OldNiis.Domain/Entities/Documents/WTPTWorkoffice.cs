using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace NIIS.DBConverter.Entities.Documents
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    [Table("WT_PT_Workoffice")]
    public class WTPTWorkoffice
    {
        [Column("U_ID")]
        public int Id { get; set; }

        [Column("date_create")]
        public DateTime? DateCreate { get; set; }

        [Column("stamp")]
        public DateTime? DateUpdate { get; set; }

        [Column("FROM_STAGE_ID")]
        public int? FromStageId { get; set; }

        [Column("FROM_USER_ID")]
        public int? FromUserId { get; set; }

        [Column("TO_STAGE_ID")]
        public int? ToStageId { get; set; }

        [Column("TO_USER_ID")]
        public int? ToUserId { get; set; }

        [Column("IS_COMPLETE")]
        public string IsComplete { get; set; }

        [Column("IS_SYSTEM")]
        public string IsSystem { get; set; }

        [Column("DESCRIPTION")]
        public string Description { get; set; }

        [Column("TYPE_ID")]
        public int? TypeId { get; set; }

        [Column("CONTROL_DATE")]
        public DateTime? ControlDate { get; set; }

        [Column("DOCUMENT_ID")]
        public int DocumentId { get; set; }

        [Column("date_reade")]
        public DateTime? DateReade { get; set; }
    }
}
