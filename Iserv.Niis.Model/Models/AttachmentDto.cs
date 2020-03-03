namespace Iserv.Niis.Model.Models
{
    public class AttachmentDto
    {
        public int TypeId { get; set; }
        public int? CopyCount { get; set; }
        public int? PageCount { get; set; }
        public string Name { get; set; }
        public string TempName { get; set; }
        public string ContentType { get; set; }
        public bool IsMain { get; set; }
        public bool WasScanned { get; set; }
    }
}
