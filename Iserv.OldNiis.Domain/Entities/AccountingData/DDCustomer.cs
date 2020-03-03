using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
// ReSharper disable InconsistentNaming

namespace NIIS.DBConverter.Entities.AccountingData
{
    [Table("DD_CUSTOMER")]
    public class DDCustomer
    {
        [Column("U_ID")]
        [Key]
        public int U_ID { get; set; }
        [Column("date_create")]
        public DateTime? date_create { get; set; }
        [Column("stamp")]
        public DateTime? stamp { get; set; }
        [Column("TYPE_ID")]
        public int TYPE_ID { get; set; }
        [Column("DOCUMENT_ID")]
        public int DOCUMENT_ID { get; set; }
        [Column("CUSTOMER_ID")]
        public int CUSTOMER_ID { get; set; }
        [Column("MENTION")]
        public string MENTION { get; set; }
        [Column("flAddressId")]
        public int? flAddressId { get; set; }
    }
}
