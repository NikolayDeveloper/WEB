using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iserv.OldNiis.Domain.Entities.Others
{
    [Table("tbDocumentUsersSignature")]
    public class TbDocumentUsersSignature
    {
        [Column("U_ID")]
        [Key]
        public int Id { get; set; }

        [Column("flDocUId")]
        public int DocId { get; set; }

        [Column("flUserId")]
        public int UserId { get; set; }

        [Column("flLut")]
        public DateTime Lut { get; set; }

        [Column("flFingerPrint")]
        public string FingerPrint { get; set; }

        [Column("flSignedData")]
        public string SignedData { get; set; }

        [Column("flSignerCertificate")]
        public string SignerCertificate { get; set; }

        [Column("flSignDate")]
        public DateTime? SignDate { get; set; }

        [Column("flSignedUserName")]
        public string SignedUserName { get; set; }
    }
}
