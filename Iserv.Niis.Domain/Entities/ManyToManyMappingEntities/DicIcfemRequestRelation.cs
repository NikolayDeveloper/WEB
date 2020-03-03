using Iserv.Niis.Domain.Entities.Dictionaries;

namespace Iserv.Niis.Domain.Entities.ManyToManyMappingEntities
{
    public class DicIcfemRequestRelation
    {
        public int DicIcfemId { get; set; }
        public DicICFEM DicIcfem { get; set; }
        public int RequestId { get; set; }
        public Request.Request Request { get; set; }
    }
}