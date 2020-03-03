using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NIIS.DBConverter.Entities.AccountingData {
    [Table("RF_CUSTOMER")]
    public class RfCustomer {
        [Column("U_ID")]
        public int Id { get; set; }

        [Column("C_TYPE")]
        public int CType { get; set; }

        [Column("DOC_ID")]
        public int DocId { get; set; }

        [Column("CUSTOMER_ID")]
        public int? CustomerId { get; set; }

        [Column("MENTION")]
        public string Mention { get; set; }

        [Column("DATE_BEGIN")]
        [DataType(DataType.Date)]
        public DateTime? DateBegin { get; set; }

        [Column("DATE_END")]
        [DataType(DataType.Date)]
        public DateTime? DateEnd { get; set; }

        [Column("ADDRESS_ID")]
        public int? AddressId { get; set; }

        [Column("date_create")]
        public DateTime? DateCreate { get; set; }

        [Column("stamp")]
        public DateTime? Stamp { get; set; }
    }
}
