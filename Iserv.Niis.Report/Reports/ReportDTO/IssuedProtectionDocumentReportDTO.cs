namespace Iserv.Niis.Report.Reports.ReportDTO
{
    internal class IssuedProtectionDocumentReportDTO
    {
        public int RowNumber { get; set; }
        public string ProtectionDocumentTypeName { get; set; }
        public int NationalCustomerCount { get; set; }
        public int NotNationalCustomerCount { get; set; }

        public bool IsPatent { get; set; }
        public bool IsCertificate { get; set; }

        public int FullReqestCountByType { get; set; }
    }
}
