using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NIIS.DBConverter.Entities.Patent {
    [Table("RF_PAT_PAT_DK")]
    public class RfPatPatDk {
        [Column("u_id")]
        [Key]
        public int u_id { get; set; }

        [Column("flParentDocId")]
        public int flParentDocId { get; set; }

        [Column("flChildDocId")]
        public int flChildDocId { get; set; }

        [Column("date_create")]
        public DateTime date_create { get; set; }

        [Column("stamp")]
        public DateTime stamp { get; set; }
    }
}
