namespace Iserv.Niis.ExternalServices.Domain.Entities
{
    public class IntegrationCalcLink
    {
        public int ActionId { get; set; }
        public bool SecondPass { get; set; }
        public int LinkId { get; set; }
        public int PatentId { get; set; }
        public int CustomerId { get; set; }
    }
}