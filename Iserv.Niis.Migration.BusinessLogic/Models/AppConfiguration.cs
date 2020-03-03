namespace Iserv.Niis.Migration.BusinessLogic.Models
{
    public class AppConfiguration
    {
        public string NiisConnectionString { get; set; }
        public string OldNiisConnectionString { get; set; }
        public string OldNiisFileConnectionString { get; set; }
        public int PackageSize { get; set; }
        public int PackageSizeForFile { get; set; }
        public int BigPackageSize { get; set; }
        public int AuthorAttachmentDocumentId { get; set; }
        public int MainExecutorId { get; set; }
        public string FileLogPath { get; set; }
    }
}
