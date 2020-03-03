using System;
using System.IO;
using System.Text.RegularExpressions;
using Aspose.Pdf.Text;
using Aspose.Words;
using Iserv.Niis.FileConverter.Abstract;
using Iserv.Niis.FileConverter.Helpers;
using Iserv.Niis.Utils.Constans;

namespace Iserv.Niis.FileConverter.Implementations
{
    public class AsposeFileConverter : IFileConverter
    {
        static AsposeFileConverter()
        {
            new License().SetLicense(AsposeLicenseHelper.Stream);
            new Aspose.Pdf.License().SetLicense(AsposeLicenseHelper.Stream);
        }

        public GeneratedDocument DocxToPdf(byte[] sourceFileBytes, string fileName)
        {
            if (sourceFileBytes == null)
            {
                throw new ArgumentNullException(nameof(sourceFileBytes));
            }

            using (var memoryStream = new MemoryStream(sourceFileBytes))
            {
                return DocxToPdf(memoryStream, fileName);
            }
        }

        public GeneratedDocument DocxToPdf(Stream sourceStream, string fileName)
        {
            if (sourceStream == null)
            {
                throw new ArgumentNullException(nameof(sourceStream));
            }

            sourceStream.Seek(0, SeekOrigin.Begin);

            Document document = null;

            try
            {
                document = new Document(sourceStream);
            }
            catch (Exception e)
            {
                throw;
            }
            

            using (var outputMemoryStream = new MemoryStream())
            {
                document.Save(outputMemoryStream, SaveFormat.Pdf);
                sourceStream.Seek(0, SeekOrigin.Begin);

                return new GeneratedDocument(ContentType.Pdf)
                {
                    File = outputMemoryStream.ToArray(),
                    PageCount = document.PageCount,
                    FileName = fileName
                };
            }
        }

        public string GetTextOfFile(byte[] file, string fileType)
        {
            switch (fileType)
            {
                case ContentType.Docx:
                    using (var memoryStream = new MemoryStream(file))
                    {
                        var document = new Document(memoryStream);
                        return document.GetText()?.Trim() ?? string.Empty;
                    }
                case ContentType.Pdf:
                    using (var memoryStream = new MemoryStream(file))
                    {
                        var document = new Aspose.Pdf.Document(memoryStream);
                        var absorber = new TextAbsorber();
                        document.Pages.Accept(absorber);
                        return absorber.Text?.Trim() ?? string.Empty;
                    }
                default:
                    return string.Empty;
            }
        }

        public int PageCount(byte[] sourceFileBytes)
        {
            using (var memoryStream = new MemoryStream(sourceFileBytes))
            {
                return PageCount(memoryStream);
            }
        }

        public int PageCount(Stream sourceStream)
        {
            sourceStream.Seek(0, SeekOrigin.Begin);
            using (Stream temp = new MemoryStream())
            {
                sourceStream.CopyTo(temp);
                temp.Seek(0, SeekOrigin.Begin);
                sourceStream.Seek(0, SeekOrigin.Begin);
                using (var reader = new StreamReader(temp))
                {
                    var regex = new Regex(@"/Type\s*/Page[^s]");
                    var matches = regex.Matches(reader.ReadToEnd());

                    return matches.Count;
                }
            }
        }
    }
}