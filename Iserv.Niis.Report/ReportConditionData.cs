using Iserv.Niis.Report.ReportFilter;
using System;
using System.Collections.Generic;

namespace Iserv.Niis.Report
{
    public sealed class ReportConditionData : ReportFilterData
    {
        /// <summary>
        /// Коды отчетов храняться в классе ReportCodes
        /// </summary>
        public string ReportCode { get; set; }
        public DateTimeOffset? DateFrom { get; set; }
        public DateTimeOffset? DateTo { get; set; }
        /// <summary>
        /// Перечисление типов заявок (объект интеллектуальной собственности)
        /// </summary>
        public List<int> ProtectionDocTypeIds { get; set; }
    }
}
