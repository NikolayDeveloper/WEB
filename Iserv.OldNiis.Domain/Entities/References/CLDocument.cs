using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace NIIS.DBConverter.Entities.References
{
    [Table("CL_DOCUMENT")]
    // ReSharper disable once InconsistentNaming
    public class CLDocument
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
        
        [Column("TEMPLATE")]
        public byte[] Template { get; set; }
        
        [Column("SYS_TEMPLATE")]
        public string FileName { get; set; }
        
        [Column("DESCRIPTION")]
        public string Description { get; set; }
        
        [Column("WORKTYPE_ID")]
        public int? WorktypeId { get; set; }
        
        [Column("IS_UNIQUE")]
        public string IsUnique { get; set; }
        
        [Column("stamp")]
        public DateTime? Stamp { get; set; }
        
        [Column("TYPE_ID")]
        public int? DocumentClassificationId { get; set; }
        
        [Column("PROCEDURE_ID")]
        public int? ProcedureId { get; set; }
        
        [Column("flOrder")]
        public int? Order { get; set; }
        
        [Column("flRequiresSigning")]
        public string IsSigningRequire { get; set; }
        
        [Column("flFingerPrintTempl")]
        public string FingerPrintTemplate { get; set; }
    }
}
