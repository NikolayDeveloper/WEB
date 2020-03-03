using System.Collections.Generic;

namespace Iserv.Niis.Model.Models.Material
{
    public class AttachMaterialDto
    {
        /// <summary>
        /// Родительский документ.
        /// </summary>
        public MaterialDetailDto Parent { get; set; }
        /// <summary>
        /// Дочерние документы.
        /// </summary>
        public List<MaterialDetailDto> Children { get; set; }
    }
}
