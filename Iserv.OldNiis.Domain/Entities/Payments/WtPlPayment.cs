using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace NIIS.DBConverter.Entities.Payments {
    [Table("WT_PL_PAYMENT")]
    public class WtPlPayment {
        [Column("U_ID")]
        public int Id { get; set; }

        [Column("date_create")]
        public DateTime? DateCreate { get; set; }

        [Column("CUSTOMER_ID")]
        public int? CustomerId { get; set; }

        [Column("PAYMENT_TYPE")]
        public string PaymentType { get; set; }

        [Column("PAYMENT_AMOUNT")]
        public decimal? Amount { get; set; }

        [Column("PAYMENT_DATE")]
        public DateTime? PaymentDate { get; set; }

        [Column("PAYMENT_NUMB")]
        public string Payment1CNumber { get; set; }

        [Column("IS_AVANS")]
        public string IsAvans { get; set; }

        [Column("DSC")]
        public string Dsc { get; set; }

        [Column("USE_DSC")]
        public string UseDsc { get; set; }

        [Column("flValSum")]
        public decimal? ValSum { get; set; }

        [Column("flExchangeRate")]
        public decimal? ExchangeRate { get; set; }

        [Column("flValType")]
        public string ValType { get; set; }
    }
}
