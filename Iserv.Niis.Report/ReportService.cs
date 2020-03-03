using Iserv.Niis.Common.Codes;
using Iserv.Niis.Report.Reports;

namespace Iserv.Niis.Report
{
    public class ReportService
    {
        public IReportBase GetReport(ReportConditionData reportFilterData)
        {
            IReportBase report = null;
            switch (reportFilterData.ReportCode)
            {
                case ReportCodes.ReceivedRequestReport:
                    report = new ReceivedRequestReport(reportFilterData);
                    break;

                case ReportCodes.IssuedProtectionDocumentsReport:
                    report = new IssuedProtectionDocumentsReport(reportFilterData);
                    break;

                default:
                    break;
            }

            return report;
        }
    }
}
