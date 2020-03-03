using System.ComponentModel.DataAnnotations.Schema;

namespace NIIS.DBConverter.Entities.Payments {
    [Table("DD_PAYMENT_DATA")]
    public class DdPaymentData {
        [Column("U_ID")]
        public int Id { get; set; }

        [Column("DOCUMENT_ID")]
        public int? DocumentId { get; set; }

        [Column("FIXPAY_ID")]
        public int? FixPayId { get; set; }
    }
}
