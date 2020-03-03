using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NIIS.DBConverter.Entities.DBObjects {
    [Table("ST_COLUMN")]
    public class StColumn {
        [Column("U_ID")]
        public int Id { get; set; }
        [Column("CODE")]
        public string Code { get; set; }
        [Column("TABLE_ID")]
        public int TableId { get; set; }
    }
}
