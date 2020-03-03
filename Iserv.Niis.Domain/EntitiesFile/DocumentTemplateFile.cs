using System;
using System.Collections.Generic;
using Iserv.Niis.Domain.Abstract;
using Iserv.Niis.Domain.Entities.Dictionaries;

namespace Iserv.Niis.Domain.EntitiesFile
{
    public class DocumentTemplateFile : Entity<int>, IEntityHasFile<int>
    {
        public string FileName { get; set; }
        public string FileType { get; set; }
        public long FileSize { get; set; }
        public byte[] File { get; set; }
        public string FileFingerPrint { get; set; }

        public ICollection<DicDocumentType> DocumentTypes { get; set; }
    }
}