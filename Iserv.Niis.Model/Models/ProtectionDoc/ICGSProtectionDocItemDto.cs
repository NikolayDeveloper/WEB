namespace Iserv.Niis.Model.Models.ProtectionDoc
{
    public class ICGSProtectionDocItemDto
    {
        public int Id { get; set; }
        public int ProtectionDocId { get; set; }
        public int IcgsId { get; set; }
        public string IcgsNameRu { get; set; }
        public string IcgsNameKz { get; set; }
        public string IcgsNameEn { get; set; }
        public string Description { get; set; }
    }
}