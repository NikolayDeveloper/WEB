using System.IO;

namespace Iserv.Niis.FileConverter.Abstract
{
    public interface IFileConverter
    {
        GeneratedDocument DocxToPdf(byte[] sourceFileBytes, string fileName);
        GeneratedDocument DocxToPdf(Stream sourceStream, string fileName);
        int PageCount(byte[] sourceFileBytes);
        int PageCount(Stream sourceStream);
        string GetTextOfFile(byte[] file, string fileType);
    }
}