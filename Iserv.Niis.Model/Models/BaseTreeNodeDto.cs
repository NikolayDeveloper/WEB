using System.Collections.Generic;

namespace Iserv.Niis.Model.Models
{
    public class BaseTreeNodeDto
    {
        public object Data { get; set; }
        public string Label { get; set; }
        public bool Selectable { get; set; }
        public bool? Leaf { get; set; }
        public IEnumerable<BaseTreeNodeDto> Children { get; set; }
    }
}