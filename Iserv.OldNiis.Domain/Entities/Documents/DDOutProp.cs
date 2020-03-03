using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace NIIS.DBConverter.Entities.Documents
{
    [Table("DD_OUT_PROP")]
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class DDOutProp {
        [Column("U_ID")]
        public int Id { get; set; }
        [Column("NETTO")]
        public double? Netto { get; set; }
        [Column("TYPE_ID")]
        public int? TypeId { get; set; }
        [Column("AMOUNT")]
        public double? Amount { get; set; }
        [Column("DOC_NUMBER")]
        public string DocNumber { get; set; }
    }
}
