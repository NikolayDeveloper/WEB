namespace Iserv.Niis.ExternalServices.Features.IntegrationIntelEGov.Models
{
    public class AttachedFileModel
    {
        public int CopyCount { get; set; }
        public int Length { get; set; }
        public byte[] File { get; set; }
        public int PageCount { get; set; }
        public string Name { get; set; }
        public bool IsMain { get; set; }
    }
}