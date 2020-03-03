using System.Collections.Generic;
using System.IO;

namespace Iserv.Niis.Business.Abstract
{
    public interface IFileExporter
    {
        MemoryStream Export<T>(IEnumerable<T> data, FileType fileType, params string[] fields);
    }

    public enum FileType
    {
        None,
        Xlsx,
        Docx,
        Pdf
    }
}