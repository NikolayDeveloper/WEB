using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

// ReSharper disable InconsistentNaming

namespace NIIS.DBConverter.Entities.References
{
    [Table("RF_TM_ICFEM")]
    public class RfTmIcfem : IEquatable<RfTmIcfem>
    {
        [Column("U_ID")]
        [Key]
        public int U_ID { get; set; }
        [Column("date_create2")]
        public string date_create2 { get; set; }
        [Column("stamp2")]
        public string stamp2 { get; set; }
        [Column("DOC_ID")]
        public int? DOC_ID { get; set; }
        [Column("LCFEM_ID")]
        public int? LCFEM_ID { get; set; }
        [Column("DOCUMENT_ID")]
        public int? DOCUMENT_ID { get; set; }
        [Column("date_create")]
        public DateTime? date_create { get; set; }
        [Column("stamp")]
        public DateTime? stamp { get; set; }

        public bool Equals(RfTmIcfem other)
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

            return Equals(obj as RfTmIcfem);
        }

        public override int GetHashCode()
        {
            int hashDocId = DOC_ID.GetHashCode();

            int hashLcfemId = LCFEM_ID.HasValue ? LCFEM_ID.HasValue.GetHashCode() : 0;

            return hashDocId ^ hashLcfemId;
        }
    }
}
