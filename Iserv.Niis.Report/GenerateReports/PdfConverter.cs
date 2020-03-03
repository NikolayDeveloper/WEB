using Aspose.Cells;
using System.IO;


namespace Iserv.Niis.Report.GenerateReports
{
    internal class PdfConverter
    {
        public byte[] ConvertExcelToPdf(string excelFilePath)
        {
            Workbook workbook = new Workbook(excelFilePath);

            workbook.Save(excelFilePath + "output.pdf", SaveFormat.Pdf);

            return File.ReadAllBytes(excelFilePath + "output.pdf");
        }
    }
}
