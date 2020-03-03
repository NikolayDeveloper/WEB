using System;
using Iserv.Niis.Domain.Abstract;

namespace Iserv.Niis.Domain.EntitiesFile
{
    public class ImageFile : IEntityHasFile<int>
    {
        public int Id { get; set; }
        public DateTimeOffset? DateCreate { get; set; }
        public DateTimeOffset? DateUpdate { get; set; }
        public string FileName { get; set; }
        public string FileType { get; set; }
        public long FileSize { get; set; }
        public byte[] File { get; set; }
        public string FileFingerPrint { get; set; }

        public override string ToString()
        {
            return FileName + " Размер файла: " + FileSize;
        }
    }
}