using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace NIIS.DBConverter.Entities.AccountingData {
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    [Table("WT_ADDRESS")]
    public class WTAddress {
        [Column("U_ID")]
        public int Id { get; set; }

        [Column("date_create")]
        public DateTime? DateCreate { get; set; }

        [Column("stamp")]
        public DateTime? DateUpdate { get; set; }

        [Column("LOCATION_ID")]
        public int? LocationId { get; set; }

        [Column("ADDRESS_ML_EN")]
        public string AddressMlEn { get; set; }

        [Column("ADDRESS_ML_RU")]
        public string AddressMllRu { get; set; }

        [Column("ADDRESS_ML_KZ")]
        public string AddressMlKz { get; set; }

        [Column("POST_BOX")]
        public string PostBox { get; set; }
    }
}
