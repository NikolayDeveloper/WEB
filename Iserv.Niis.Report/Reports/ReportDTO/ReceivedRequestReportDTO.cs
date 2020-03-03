using System.Collections.Generic;

namespace Iserv.Niis.Report.Reports.ReportDTO
{
    internal class ReceivedRequestReportDTO
    {
        public int RowNumber { get; set; }
        public string RequestTypeName { get; set; }
        public int NationalCustomerRequestCount { get; set; }
        public int NotNationalCustomerRequestCount { get; set; }
        public int FullReqestCountByType { get; set; }

        /// <summary>
        /// Маппинг для отчета "Сведения по поступившим в РГП «НИИС» заявкам на выдачу охранных документов на объекты промышленной собственности за год"
        /// </summary>
        public Row MapReceivedRequestReport()
        {
            return new Row
            {
                Cells = new List<Cell>
                {
                    new Cell { Value = RowNumber },
                    new Cell { Value = RequestTypeName },
                    new Cell { Value = NationalCustomerRequestCount },
                    new Cell { Value = NotNationalCustomerRequestCount },
                    new Cell { Value = FullReqestCountByType },
                }
            };
        }
    }
}
