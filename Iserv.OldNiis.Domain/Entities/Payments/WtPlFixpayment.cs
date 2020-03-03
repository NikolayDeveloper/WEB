using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace NIIS.DBConverter.Entities.Payments {
    [Table("WT_PL_FIXPAYMENT")]
    public class WtPlFixpayment {
        [Column("U_ID")]
        public int Id { get; set; }

        [Column("TARIFF_ID")]
        public int TariffId { get; set; }

        [Column("FINE_PERCENT")]
        public decimal FinePercent { get; set; }

        [Column("VAT_PERCENT")]
        public decimal VatPercent { get; set; }

        [Column("APP_ID")]
        public int AppId { get; set; }

        [Column("date_create")]
        public DateTime? DateCreate { get; set; }

        [Column("stamp")]
        public DateTime? Stamp { get; set; }

        [Column("TARIFFAMOUNT")]
        public decimal? TariffAmount { get; set; }

        [Column("PENI_PERCENT")]
        public decimal PeniPercent { get; set; }

        [Column("TARIFF_COUNT")]
        public int? TariffCount { get; set; }

        [Column("IS_COMPLETE")]
        public string IsComplete { get; set; }

        [Column("DATE_LIMIT")]
        public DateTime? DateLimit { get; set; }

        [Column("DATE_FACT")]
        public DateTime? DateFact { get; set; }

        [Column("DATE_COMPLETE")]
        public DateTime? DateComplete { get; set; }

        [Column("flCreateUserId")]
        public int? CreateUserId { get; set; }
    }
}
