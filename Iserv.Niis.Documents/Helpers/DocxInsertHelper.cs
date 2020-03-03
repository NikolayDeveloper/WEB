using System.Drawing;
using Aspose.Words;

namespace Iserv.Niis.Documents.Helpers
{
    public class DocxInsertHelper
    {
        public void InsertImageToHeader(Image image, DocumentBuilder builder)
        {
            builder.MoveToHeaderFooter(HeaderFooterType.HeaderPrimary);
            builder.ParagraphFormat.Alignment = ParagraphAlignment.Right;
            builder.InsertImage(image, 90, 90);
        }

        public void InsertImageToFooter(Image image, DocumentBuilder builder)
        {
            builder.MoveToHeaderFooter(HeaderFooterType.FooterPrimary);
            builder.InsertImage(image, 90, 90);
        }
    }
}
