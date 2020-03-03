using System;

namespace Iserv.Niis.Business.Models
{
    public class DocumentFileInfo
    {
        public int Id { get; set; }
        public string TypeCode{ get; set; }
        public string BucketName { get; set; }
        public string OriginalName { get; set; }
        public string ContentType { get; set; }
        public DateTimeOffset DateUpdate { get; set; }
        public bool? IsFinished { get; set; }
    }
}