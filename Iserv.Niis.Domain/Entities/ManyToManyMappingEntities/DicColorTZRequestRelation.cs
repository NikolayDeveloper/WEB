using Iserv.Niis.Domain.Entities.Dictionaries;

namespace Iserv.Niis.Domain.Entities.ManyToManyMappingEntities
{
    public class DicColorTZRequestRelation
    {
        public int ColorTzId { get; set; }
        public DicColorTZ ColorTz { get; set; }
        public int RequestId { get; set; }
        public Request.Request Request { get; set; }
    }
}