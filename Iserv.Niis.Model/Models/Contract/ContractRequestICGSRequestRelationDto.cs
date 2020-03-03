using Iserv.Niis.Model.Models.Request;

namespace Iserv.Niis.Model.Models.Contract
{
    public class ContractRequestICGSRequestRelationDto
    {
        public int Id { get; set; }
        public int ContractRequestRelationId { get; set; }
        public int ICGSRequestId { get; set; }
        public ICGSRequestItemDto ICGSRequest { get; set; }
        public string[] Descriptions { get; set; }
    }
}