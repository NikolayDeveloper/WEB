using NIIS.DBConverter.Entities.Patent;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace NIIS.DBConverter.Entities.Documents {
    [Table("DD_DOCUMENT")]
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class DDDocument {
        [Column("U_ID")]
        public int Id { get; set; }
        [Column("date_create")]
        public DateTime? DateCreate { get; set; }
        [Column("stamp")]
        public DateTime? DateUpdate { get; set; }

        [ForeignKey("Patent")]
        [Column("DOC_ID")]
        public int? DocId { get; set; }
        public BtBasePatent Patent { get;set;}
        [Column("DOCTYPE_ID")]
        public int DocTypeId { get; set; }
        [Column("DOCUM_NUM")]
        public string DocNum { get; set; }
        [Column("DOCUM_DATE")]
        public DateTime? DocDate { get; set; }
        [Column("DOCUM_INTERVAL")]
        public DateTime? DocInterval { get; set; }
        [Column("USER_ID")]
        public int? UserId { get; set; }
        [Column("TEXT_DATA")]
        public string TextData { get; set; }
        [Column("PAGE_COUNT")]
        public int? PageCount { get; set; }
        [Column("COPY_COUNT")]
        public int? CopyCount { get; set; }
        [Column("CUSTOMER_ID")]
        public int? CustomerId { get; set; }
        [Column("SENDTYPE")]
        public int SendType { get; set; }
        [Column("DIVISION_ID")]
        public int? DivisionId { get; set; }
        [Column("CONTROL_DATE")]
        public DateTime? ControlDate { get; set; }
        [Column("ADDRESS_ID")]
        public int? AddressId { get; set; }

        [Column("IS_CONTROL")]
        public string IsControl { get; set; }

        [Column("DESCRIPTION_ML_EN")]
        public string DescMlEn { get; set; }
        [Column("DESCRIPTION_ML_RU")]
        public string DescMlRu { get; set; }
        [Column("DESCRIPTION_ML_KZ")]
        public string DescMlKz { get; set; }
        [Column("CONTROL_TEXT")]
        public string ControlText { get; set; }
        [Column("INOUT_NUM")]
        public string InOutNum { get; set; }
        [Column("CONTROL_CDATE")]
        public DateTime? ControlCDate { get; set; }
        [Column("IS_CCONTROL")]
        public string IsCControl { get; set; } 
        [Column("OUT_INFO")]
        public int? OutInfo { get; set; }
        [Column("DEPARTMENT_ID")]
        public int? DepartmentId { get; set; }
        [Column("DOC_WEIGTH")]
        public double? DocWeigth { get; set; }
        [Column("DOC_NUM_LETTER")]
        public string DocNumLetter { get; set; }
        [Column("DOC_SEND_TYPE")]
        public int? DocSendType { get; set; }
        [Column("DOC_SEND_AMMOUNT")]
        public double? DocSendAmount { get; set; }
        [Column("OUTNUM")]
        public string OutNum { get; set; }
        [Column("DOC_SEND_KONV")]
        public double? DocSendConv { get; set; }
        [Column("INNUM_ADD")]
        public string InNumAdd { get; set; }
        [Column("IS_COMPLETE")]
        public string IsComplete { get; set; }
        [Column("YEAR_CONTIN")]
        public int? YearContIn { get; set; }
        [Column("flDivisionId")]
        public int? FlDivisionId { get; set; }
        
    }
}