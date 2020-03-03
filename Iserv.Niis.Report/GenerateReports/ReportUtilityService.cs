namespace Iserv.Niis.Report.GenerateReports
{
    internal static class ReportUtilityService
    {
        public static ExcelGenerator ExcelGenerator = new ExcelGenerator();

        public static PdfConverter PdfConverter = new PdfConverter();
    }
}
