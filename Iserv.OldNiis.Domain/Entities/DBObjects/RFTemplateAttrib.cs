using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NIIS.DBConverter.Entities.DBObjects {
    [Table("RF_TEMPLATE_ATTRIB")]
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public class RFTemplateAttrib {
        [Column("U_ID")]
        public int Id { get; set; }

        [Column("TEMPLATE_ID")]
        public int TemplateId { get; set; }

        [Column("ATTRIBUTE_ID")]
        public int AttributeId { get; set; }
    }
}
