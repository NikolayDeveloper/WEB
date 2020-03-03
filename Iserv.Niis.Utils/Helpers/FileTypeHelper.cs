using Iserv.Niis.Utils.Constans;
using System;

namespace Iserv.Niis.Utils.Helpers
{
    public static class FileTypeHelper
    {

        public static string GetContentType(string fileName)
        {
            var fileExtension = GetFileExtension(fileName);
            switch (true)
            {
                case bool x when FileTypes.Pdf.Equals(fileExtension, StringComparison.CurrentCultureIgnoreCase):
                    return ContentType.Pdf;
                case bool x when FileTypes.Png.Equals(fileExtension, StringComparison.CurrentCultureIgnoreCase):
                    return ContentType.Png;
                case bool x when FileTypes.Jpeg.Equals(fileExtension, StringComparison.CurrentCultureIgnoreCase):
                    return ContentType.Jpeg;
                case bool x when FileTypes.Docx.Equals(fileExtension, StringComparison.CurrentCultureIgnoreCase):
                    return ContentType.Docx;
                case bool x when FileTypes.Doc.Equals(fileExtension, StringComparison.CurrentCultureIgnoreCase):
                    return ContentType.Doc;
                default:
                    return ContentType.Pdf;
            }
        }

        public static string GetFileExtension(string fileName)
        {
            var dividedFileName = fileName.Split('.');
            if (dividedFileName.Length > 1)
            { 
                return dividedFileName[dividedFileName.Length - 1];
            }
            return string.Empty;
        }
    }
}
