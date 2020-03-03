namespace Iserv.Niis.ExternalServices.Domain.Entities
{
    public class Reference
    {
        public int Id { get; set; }
        public string Ru { get; set; }
        public int? ParentId { get; set; }
        public string Type { get; set; }
    }
}