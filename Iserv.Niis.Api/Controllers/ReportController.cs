using Iserv.Niis.Report;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Iserv.Niis.Utils.Constans;

namespace Iserv.Niis.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    public class ReportController : BaseNiisApiController
    {
        private ReportService reportService = new ReportService();

        [HttpGet]
        public ActionResult GetAllReports()
        {
            var reports = new List<ReportDto>
            {
                new ReportDto
                {
                    Code= Common.Codes.ReportCodes.ReceivedRequestReport,
                    Name = "Сведения по поступившим в РГП «НИИС» заявкам на выдачу охранных документов на объекты промышленной собственности за указанный период"
                },
                new ReportDto
                {
                    Code= Common.Codes.ReportCodes.IssuedProtectionDocumentsReport,
                    Name = "Сведения по выданным охранным документам на объекты промышленной собственности РГП «НИИС» за указанный период"
                }
            };

            return Ok(reports);
        }

        [HttpPost]
        public ActionResult GetReportData([FromBody] ReportConditionData reportConditionData)
        {
            var report = reportService.GetReport(reportConditionData);
            var data = report.GetData();
            return Ok(data);
        }
        [HttpPost]
        public ActionResult GetChartData([FromBody] ReportConditionData reportConditionData)
        {
            var report = reportService.GetReport(reportConditionData);
            var data = report.GetChartData();
            return Ok(data);
        }

        [HttpPost]
        public FileResult GetReportAsExcel([FromBody] ReportConditionData reportConditionData)
        {
            var report = reportService.GetReport(reportConditionData);
            var excelBytes = report.GetAsExcel();

            return File(excelBytes, ContentType.Xlsx, reportConditionData.ReportCode + ".xlsx");
        }

        [HttpPost]
        public FileResult GetReportAsPdf([FromBody] ReportConditionData reportConditionData)
        {
            var report = reportService.GetReport(reportConditionData);
            var pdfBytes = report.GetAsPDF();
            //TODO: не бесплатная версия... Evaluation Only. Created with Aspose.Cells for .NET.Copyright 2003 - 2018 Aspose Pty Ltd.
            return File(pdfBytes, System.Net.Mime.MediaTypeNames.Application.Octet, reportConditionData.ReportCode + ".pdf");
        }
    }
}