using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iserv.Niis.Model.Models.Material.Internal
{
    public class MaterialInternalDetailDto: MaterialDetailDto
    {
        public int? Barcode { get; set; }
        public int? TypeId { get; set; }
        public DateTimeOffset? DateCreate { get; set; }
        public DateTimeOffset? DocumentDate { get; set; }
        public UserInputDto UserInput { get; set; }
        public string NameRu { get; set; }
        public string NameEn { get; set; }
        public string NameKz { get; set; }
        public bool WasScanned { get; set; }
        public bool HasTemplate { get; set; }
    }
}
