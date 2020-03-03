using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NIIS.DBConverter.Entities.Others {
    [Table("tbGeneratedQueryExpDep")]
    public class TbGeneratedQueryExpDep {
        [Column("flId")]
        [Key]
        public int Id { get; set; }

        [Column("flCreateDate")]
        public DateTime? CreateDate { get; set; }

        [Column("flDepartmentId")]
        public int? DepartmentId { get; set; }

        [Column("flDepartmentCode")]
        public string DepartmentCode { get; set; }

        [Column("flDocumentId")]
        public int? DocumentId { get; set; }

        [Column("flIndex")]
        public int Index { get; set; }

        [Column("flNumber")]
        public string Number { get; set; }
    }
}
