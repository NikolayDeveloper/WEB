namespace Iserv.Niis.FileConverter
{
    public class GeneratedDocument
    {
        public GeneratedDocument(string contentType)
        {
            ContentType = contentType;
        }
        public byte[] File { get; set; }
        public string FileName { get; set; }
        public int PageCount { get; set; }
        public string ContentType { get; }
        public int Length => File.Length;
    }
}