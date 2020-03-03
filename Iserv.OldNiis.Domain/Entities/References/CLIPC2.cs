using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iserv.OldNiis.Domain.Entities.References
{
    [Table("CL_IPC2")]
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class CLIPC2
    {
        [Column("U_ID")]
        public int Id { get; set; }
        [Column("F_ID")]
        public int? ParentId { get; set; }
        [Column("REDACTION_NUM")]
        public int RevisionId { get; set; }
        [Column("CODE")]
        public string Code { get; set; }
        [Column("NAME_ML_EN")]
        public string NameEn { get; set; }
        [Column("NAME_ML_RU")]
        public string NameRu { get; set; }
        [Column("NAME_ML_KZ")]
        public string NameKz { get; set; }
        [Column("FULL_DESC")]
        public string Description { get; set; }
        [Column("EntryType")]
        public string EntryType { get; set; }
        [Column("Kind")]
        public string Kind { get; set; }
        [Column("EntryLevel")]
        public string EntryLevel { get; set; }
    }
}
