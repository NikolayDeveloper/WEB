using Iserv.Niis.Domain.Abstract;
using Iserv.Niis.Domain.Entities.Dictionaries;

namespace Iserv.Niis.Domain.Entities.BibliographicData
{
    public class MadeChange: Entity<int>
    {
        public int? RequestId { get; set; }
        public Request.Request Request { get; set; }
        public int FromStageId { get; set; }
        public DicRouteStage FromStage { get; set; }
        public int ChangeTypeId { get; set; }
        public DicBiblioChangeType ChangeType { get; set; }
    }
}
