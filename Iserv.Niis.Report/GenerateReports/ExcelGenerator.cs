using Iserv.Niis.DI;
using NPOI.XSSF.UserModel;
using System;
using System.IO;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using NPOI.HSSF.Util;

namespace Iserv.Niis.Report.GenerateReports
{
    internal class ExcelGenerator
    {
        protected string GeneratedExcelFilePath = "";

        internal byte[] GenerateReportDataToExcel(ReportData reportData, string templateName, int rowStart, int columnStart)
        {
            var config = NiisAmbientContext.Current.Configuration.GetSection("ReportOptions");
            var tempFolderName = config["TempFolder"];
            var reportTemplatesFolder = config["ReportTemplatesFolder"];
            var emptyTempplateFolderName = config["EmptyReportTemplateName"];

            var tempFileFullPath = string.Format("{0}\\{1}", Path.GetFullPath(tempFolderName), Guid.NewGuid() + "Report.xlsx");

            var reportTemplateFolderFullPath = string.Format("{0}\\{1}", Path.GetFullPath(reportTemplatesFolder),
                string.IsNullOrEmpty("templateName")
                ? emptyTempplateFolderName
                : templateName);

            GeneratedExcelFilePath = tempFileFullPath;

            if (!File.Exists(tempFileFullPath))
            {

                FileStream templateFile = new FileStream(reportTemplateFolderFullPath, FileMode.Open, FileAccess.ReadWrite);
                var book = new XSSFWorkbook(templateFile);
                var sheet = (XSSFSheet)book.GetSheet("Отчет");

                var rows = reportData.Rows;

                #region Cell border styling

                var boldFont = book.CreateFont();
                boldFont.Boldweight = (short)FontBoldWeight.Bold;
                ICellStyle headerStyle = book.CreateCellStyle();
                headerStyle.SetFont(boldFont);
                headerStyle.FillForegroundColor = IndexedColors.SeaGreen.Index;
                headerStyle.FillPattern = FillPattern.SolidForeground;

                headerStyle.VerticalAlignment = VerticalAlignment.Center;
                headerStyle.Alignment = HorizontalAlignment.Center;
                headerStyle.WrapText = true;
                addThinBorder(headerStyle);

                ICellStyle simpleBorderedStyle = book.CreateCellStyle();
                addThinBorder(simpleBorderedStyle);

                ICellStyle simpleBorderedCenterAlignmentStyle = book.CreateCellStyle();
                addThinBorder(simpleBorderedStyle);
                simpleBorderedCenterAlignmentStyle.VerticalAlignment = VerticalAlignment.Center;
                simpleBorderedCenterAlignmentStyle.Alignment = HorizontalAlignment.Center;
                simpleBorderedCenterAlignmentStyle.WrapText = true;

                ICellStyle simpleBorderedBoldStyle = book.CreateCellStyle();
                simpleBorderedBoldStyle.SetFont(boldFont);
                addThinBorder(simpleBorderedBoldStyle);

                void addThinBorder(ICellStyle cellStyle)
                {

                    cellStyle.BorderLeft = BorderStyle.Thin;
                    cellStyle.BorderTop = BorderStyle.Thin;
                    cellStyle.BorderRight = BorderStyle.Thin;
                    cellStyle.BorderBottom = BorderStyle.Thin;
                }

                #endregion

                var rowNumber = rowStart;

                foreach (var row in reportData.Rows)
                {
                    var createdRow = sheet.CreateRow(rowNumber);
                    var cellNumer = columnStart;

                    #region Создание ячеек

                    if (row.Cells != null)
                    {
                        foreach (var cell in row.Cells)
                        {
                            var createdCell = createdRow.CreateCell(cellNumer);
                            if (cell.Value != null)
                            {
                                createdCell.SetCellValue(cell.Value);
                            }

                            //усановим стайлинг для шапки/остальных строк
                            if (row.IsHeader)
                            {
                                createdCell.CellStyle = headerStyle;
                                sheet.AutoSizeColumn(cellNumer, true);
                            }
                            else
                            {
                                createdCell.CellStyle = simpleBorderedStyle;

                                if (cell.IsBold)
                                {
                                    createdCell.CellStyle = simpleBorderedBoldStyle;
                                }
                                else if (cell.IsTextAlignCenter)
                                {
                                    createdCell.CellStyle = simpleBorderedCenterAlignmentStyle;
                                }
                            }

                            //объединение ячеек
                            if (cell.CollSpan > 0 || cell.RowSpan > 0)
                            {
                                var lastRowIndex = cell.RowSpan > 0 ? rowNumber + cell.RowSpan - 1 : 0;
                                var lastColumnIndex = cell.CollSpan > 0 ? cellNumer + cell.CollSpan - 1 : 0;

                                if (lastRowIndex < rowNumber)
                                {
                                    lastRowIndex = rowNumber;
                                }
                                if (lastColumnIndex < cellNumer)
                                {
                                    lastColumnIndex = cellNumer;
                                }
                                var cellRangeAddress = new CellRangeAddress(rowNumber, lastRowIndex, cellNumer, lastColumnIndex);
                                sheet.AddMergedRegion(cellRangeAddress);

                                RegionUtil.SetBorderBottom((int)BorderStyle.Thin, cellRangeAddress, sheet, book);
                                RegionUtil.SetBorderTop((int)BorderStyle.Thin, cellRangeAddress, sheet, book);
                            }                           

                            if (cell.CollSpan > 0)
                            {
                                cellNumer = cellNumer + cell.CollSpan;
                            }
                            else
                            {
                                cellNumer++;
                            }
                        }
                    }

                    #endregion

                    rowNumber++;
                }

                using (var fileStreem = new FileStream(tempFileFullPath, FileMode.Create, FileAccess.Write))
                {
                    book.Write(fileStreem);
                }
            }

            return File.ReadAllBytes(tempFileFullPath);
        }
    }
}
