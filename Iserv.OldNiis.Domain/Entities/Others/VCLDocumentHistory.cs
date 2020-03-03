using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NIIS.DBConverter.Entities.Others {
    [Table("CL_DOCUMENT_History")]
    public class VCLDocumentHistory {
        [Column("U_ID")]
        public int HistoryId { get; set; }

        [Column("flHistoryDate")]
        public DateTime HistoryDate { get; set; }

        [Column("flHistoryType")]
        public int HistoryType { get; set; }

        [Column("flUserId")]
        public int UserId { get; set; }

        [Column("flIpAddress")]
        public string IpAddress { get; set; }

        [Column("flU_ID")]
        public int Id { get; set; }

        [Column("CODE")]
        public string Code { get; set; }

        [Column("NAME_ML_EN")]
        public string NameEn { get; set; }

        [Column("NAME_ML_RU")]
        public string NameRu { get; set; }

        [Column("NAME_ML_KZ")]
        public string NameKz { get; set; }

        [Column("TEMPLATE")]
        public byte[] Template { get; set; }

        [Column("SYS_TEMPLATE")]
        public string SysTemplate { get; set; }

        [Column("DESCRIPTION")]
        public string Description { get; set; }

        [Column("WORKTYPE_ID")]
        public int? WorkTypeId { get; set; }

        [Column("IS_UNIQUE")]
        public string IsUnique { get; set; }

        [Column("stamp")]
        public DateTime? Stamp { get; set; }

        [Column("TYPE_ID")]
        public int? TypeId { get; set; }

        [Column("PROCEDURE_ID")]
        public int? ProcId { get; set; }

        [Column("flOrder")]
        public int? Order { get; set; }

        [Column("flFingerPrintTempl")]
        public string FingerPrintTempl { get; set; }

        [Column("flRequiresSigning")]
        public string RequiresSigning { get; set; }
    }
}
