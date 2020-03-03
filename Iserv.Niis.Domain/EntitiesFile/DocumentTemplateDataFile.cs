using System;
using Iserv.Niis.Domain.Abstract;
using Iserv.Niis.Domain.EntitiesHistory.EntitiesFile;

namespace Iserv.Niis.Domain.EntitiesFile
{
    public class DocumentTemplateDataFile : IEntityHasFile<int>, IHistorySupport
    {
        public int Id { get; set; }
        public DateTimeOffset? DateCreate { get; set; }
        public DateTimeOffset? DateUpdate { get; set; }
        public string FileName { get; set; }
        public string FileType { get; set; }
        public long FileSize { get; set; }
        public byte[] File { get; set; }
        public string FileFingerPrint { get; set; }

        public Type GetHistoryEntity()
        {
            return typeof(DocumentTemplateDataHistory);
        }
    }
}