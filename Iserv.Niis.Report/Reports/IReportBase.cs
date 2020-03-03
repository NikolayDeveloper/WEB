using Iserv.Niis.Report.Chart;

namespace Iserv.Niis.Report.Reports
{
    public interface IReportBase
    {
        ReportData GetData();
        byte[] GetAsExcel();
        byte[] GetAsPDF();
        ChartDataBase GetChartData();
    }
}
