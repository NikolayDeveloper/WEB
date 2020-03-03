namespace Iserv.Niis.Model.Models
{
    public class SelectOptionDto
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string NameRu { get; set; }
        public string DicType { get; set; }
        public bool? IsDeleted { get; set; }
    }
}