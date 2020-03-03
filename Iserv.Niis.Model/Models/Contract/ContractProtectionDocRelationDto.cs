using Iserv.Niis.Model.Models.ProtectionDoc;

namespace Iserv.Niis.Model.Models.Contract
{
    public class ContractProtectionDocRelationDto
    {
        public int Id { get; set; }
        public int ContractId { get; set; }
        public ProtectionDocItemDto ProtectionDoc { get; set; }
        public ContractProtectionDocICGSProtectionDocRelationDto[] ICGSProtectionDocs { get; set; }
    }
}