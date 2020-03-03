using Iserv.Niis.Domain.Abstract;
using Iserv.Niis.Domain.Entities.Dictionaries;

namespace Iserv.Niis.Domain.Entities.Other
{
    public class AvailabilityCorrespondence : Entity<int>
    {
        public int ProtectionDocTypeId { get; set; }
        public DicProtectionDocType ProtectionDocType { get; set; }
        public int RouteStageId { get; set; }
        public DicRouteStage RouteStage { get; set; }
        public int DocumentTypeId { get; set; }
        public DicDocumentType DocumentType { get; set; }
        public int Status { get; set; }
    }
}