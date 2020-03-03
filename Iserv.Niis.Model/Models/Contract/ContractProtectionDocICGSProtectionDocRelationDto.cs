using Iserv.Niis.Model.Models.ProtectionDoc;

namespace Iserv.Niis.Model.Models.Contract
{
    public class ContractProtectionDocICGSProtectionDocRelationDto
    {
        public int Id { get; set; }
        public int ContractProtectionDocRelationId { get; set; }
        public ICGSProtectionDocItemDto ICGSProtectionDoc { get; set; }
        public string Description { get; set; }
    }
}