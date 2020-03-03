using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iserv.OldNiis.Domain.Entities
{
    [Table("IntegrationConServiceStatus")]
    public class IntegrationConServiceStatus
    {
        public int Id { get; set; }
        public int Code { get; set; }
        public string NameRu { get; set; }
        public string NameKz { get; set; }
        public string NameEn { get; set; }
        public DateTime DateCreate { get; set; }
        public DateTime DateUpdate { get; set; }
    }
}
