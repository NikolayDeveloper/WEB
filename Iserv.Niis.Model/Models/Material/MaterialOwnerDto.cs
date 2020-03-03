using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Iserv.Niis.Domain.Helpers;

namespace Iserv.Niis.Model.Models.Material
{
    public class MaterialOwnerDto
    {
        public int OwnerId { get; set; }
        public int? ProtectionDocTypeId { get; set; }
        public Owner.Type OwnerType { get; set; }
    }
}
