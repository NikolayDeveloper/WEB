using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// ReSharper disable InconsistentNaming

namespace NIIS.DBConverter.Entities.Others
{
    [Table("RF_TM_ICOLOR_TM")]
    public class RfTmIColorTm : IEquatable<RfTmIColorTm>
    {
        [Column("U_ID")]
        [Key]
        public int U_ID { get; set; }
        [Column("date_create")]
        public DateTime? date_create { get; set; }
        [Column("stamp")]
        public DateTime? stamp { get; set; }
        [Column("DOC_ID")]
        public int? DOC_ID { get; set; }
        [Column("LCFEM_ID")]
        public int LCFEM_ID { get; set; }

        public bool Equals(RfTmIColorTm other)
        {
            if (other == null)
            {
                return false;
            }

            return DOC_ID == other.DOC_ID
                && LCFEM_ID == other.LCFEM_ID;
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            return Equals(obj as RfTmIColorTm);
        }

        public override int GetHashCode()
        {
            var hashDocId = DOC_ID.HasValue ? DOC_ID.Value.GetHashCode() : 0;
            var hashLcfemId = LCFEM_ID.GetHashCode();
            return hashDocId ^ hashLcfemId;
        }
    }
}
