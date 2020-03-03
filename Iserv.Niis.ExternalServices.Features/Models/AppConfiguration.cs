using System.Collections.Generic;

namespace Iserv.Niis.ExternalServices.Features.Models
{
    public class AppConfiguration
    {
        public string UrlServiceKazPatent { get; set; }
        public string LogXmlDir { get; set; }
        public string ConStringNiisWeb { get; set; }
        public string ConStringNiisIntegration { get; set; }
        public string ServerPepIp { get; set; }
        public string FolderRequisitionFile { get; set; }
        public int AuthorAttachmentDocumentId { get; set; }
        public string HashPassword { get; set; }
        public Dictionary<string, int> MainExecutorIds { get; set; }
    }
}
