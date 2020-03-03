using Iserv.Niis.Model.Models.Request;

namespace Iserv.Niis.Model.Models.Contract
{
    public class ContractRequestRelationDto
    {
        public int Id { get; set; }
        public int ContractId { get; set; }
        public RequestItemDto Request { get; set; }
        public ICGSRequestItemDto[] IcgsRequestItemDtos { get; set; }
        public ContractRequestICGSRequestRelationDto[] ICGSRequestRelations { get; set; }
    }
}