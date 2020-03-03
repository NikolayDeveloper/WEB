using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace NIIS.DBConverter.Entities.Payments {
    [Table("WT_PL_PAYMENT_USE")]
    public class WtPlPaymentUse {
        [Column("U_ID")]
        public int Id { get; set; }

        [Column("PAYMENT_ID")]
        public int? PaymentId { get; set; }

        [Column("FIX_ID")]
        public int? FixId { get; set; }

        [Column("AMOUNT")]
        public decimal Amount { get; set; }

        [Column("date_create")]
        public DateTime? DateCreate { get; set; }

        [Column("DSC")]
        public string Dsc { get; set; }

        [Column("flCreateUserId")]
        public int? CreateUserId { get; set; }
    }
}
