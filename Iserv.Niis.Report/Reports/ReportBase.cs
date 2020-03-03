using Iserv.Niis.Report.GenerateReports;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; ;
using Iserv.Niis.DI;
using Iserv.Niis.Report.ReportBusinessLogic;
using System.Collections.Generic;
using System.Drawing;
using Iserv.Niis.Report.Chart;

namespace Iserv.Niis.Report.Reports
{
    internal abstract class ReportBase<TReportQuery> : ExcelGenerator, IReportBase
        where TReportQuery : BaseReportQuery
    {
        protected IExecutor Executor = NiisAmbientContext.Current.Executor;

        protected ReportConditionData ReportConditionData = null;
        protected abstract string ReportTemplate { get; }
        protected abstract int RowStart { get; }
        protected abstract int ColumnStart { get; }

        private ReportData _reportData = null;
        
        private string ExcelFilePath
        {
            get
            {
                if (string.IsNullOrEmpty(GeneratedExcelFilePath))
                {
                    GetAsExcel();
                    return GeneratedExcelFilePath;
                }

                return GeneratedExcelFilePath;
            }
        }

        private ReportData ReportData
        {
            get
            {
                if (_reportData == null)
                {
                    _reportData = GetReportData();
                    return _reportData;
                }

                return _reportData;
            }
        }

        public ReportBase(ReportConditionData rportConditionData)
        {
            ReportConditionData = rportConditionData;
        }

        public ReportData GetData()
        {
            return ReportData;
        }

        public abstract ChartDataBase GetChartData();

        public byte[] GetAsExcel()
        {
            return ReportUtilityService.ExcelGenerator.GenerateReportDataToExcel(ReportData, ReportTemplate, RowStart, ColumnStart);
        }

        public byte[] GetAsPDF()
        {
            return ReportUtilityService.PdfConverter.ConvertExcelToPdf(ExcelFilePath);
        }

        private ReportData GetReportData()
        {
            var reportData = Executor.GetQuery<TReportQuery>().Process(r => r.Execute(ReportConditionData));

            var headerRows = GetReportHeader();
            foreach (var row in headerRows)
            {
                reportData.Rows.Insert(0, row);
            }

            return reportData;
        }

        protected abstract List<Row> GetReportHeader();
    }
}
