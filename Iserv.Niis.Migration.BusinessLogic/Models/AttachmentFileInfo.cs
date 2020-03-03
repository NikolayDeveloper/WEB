using System;

namespace Iserv.Niis.Migration.BusinessLogic.Models
{
    public class AttachmentFileInfo
    {
        public int Id { get; set; }
        public DateTimeOffset DateCreate { get; set; }
        public DateTimeOffset DateUpdate { get; set; }
        public string FileName { get; set; }
        public byte[] File { get; set; }
    }
}
