using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using Iserv.Niis.Business.Abstract;
using Iserv.Niis.Domain.Enums;
using Iserv.Niis.Domain.Helpers;

namespace Iserv.Niis.Business.Implementations
{
    public class FileExporter : IFileExporter
    {
        private const int MaxRowCount = 1_048_576;

        private static readonly Dictionary<Type, CellValues> CellValueTypeDictionary = new Dictionary<Type, CellValues>
        {
            {typeof(string), CellValues.String},
            {typeof(DateTimeOffset), CellValues.String},
            {typeof(DateTimeOffset?), CellValues.String},
            {typeof(bool), CellValues.Boolean},
            {typeof(bool?), CellValues.Boolean},
            {typeof(int), CellValues.Number},
            {typeof(Owner.Type), CellValues.String},
            {typeof(DocumentType), CellValues.String},
            {typeof(int?), CellValues.Number},
            {typeof(decimal), CellValues.Number},
            {typeof(decimal?), CellValues.Number}
        };

        public MemoryStream Export<T>(IEnumerable<T> data, FileType fileType, params string[] fields)
        {
            switch (fileType)
            {
                case FileType.Pdf:
                case FileType.Docx:
                    throw new NotImplementedException();
                case FileType.Xlsx:
                    return ExportToExcel(data.Take(MaxRowCount).ToList(), fields);
                default:
                    throw new ArgumentOutOfRangeException(nameof(fileType), fileType, null);
            }
        }

        private MemoryStream ExportToExcel<T>(IList<T> data, string[] fields)
        {
            var stream = new MemoryStream();
            using (var document = SpreadsheetDocument.Create(stream, SpreadsheetDocumentType.Workbook, true))
            {
                var workbookPart = document.AddWorkbookPart();
                workbookPart.Workbook = new Workbook();

                var worksheetPart = workbookPart.AddNewPart<WorksheetPart>();
                worksheetPart.Worksheet = new Worksheet();

                var sheets = workbookPart.Workbook.AppendChild(new Sheets());
                var sheet = new Sheet
                {
                    Id = workbookPart.GetIdOfPart(worksheetPart),
                    SheetId = 1,
                    Name = "Результаты"
                };
                sheets.Append(sheet);

                workbookPart.Workbook.Save();

                var sheetData = worksheetPart.Worksheet.AppendChild(new SheetData());
                var displayProperties = GetDisplayProperties<T>(fields).ToList();

                // Insert the header row to the Sheet Data
                sheetData.AppendChild(GetHeaderRow<T>(displayProperties));

                // Inserting each employee
                foreach (var element in data)
                    sheetData.AppendChild(GetRow(displayProperties, element));

                worksheetPart.Worksheet.Save();
            }
            stream.Seek(0, SeekOrigin.Begin);

            return stream;
        }

        private Row GetHeaderRow<T>(IList<PropertyInfo> displayProperties)
        {
            var row = new Row();

            row.Append(displayProperties.Select(dp =>
                ConstructCell(dp.GetCustomAttribute<DisplayAttribute>().Name, CellValues.String)));

            return row;
        }

        private Row GetRow<T>(IList<PropertyInfo> displayProperties, T element)
        {
            var row = new Row();

            row.Append(displayProperties.Select(dp =>
                ConstructCell(dp.GetValue(element, null), CellValueTypeDictionary[dp.PropertyType])));

            return row;
        }

        private static IEnumerable<PropertyInfo> GetDisplayProperties<T>(string[] fields)
        {
            var result = new List<PropertyInfo>();
            var properties = typeof(T).GetProperties()
                    .Where(p => fields.Length == 0
                        ? p.CustomAttributes.Any(a => a.AttributeType == typeof(DisplayAttribute))
                        : fields.Any(f => f.Equals(p.Name, StringComparison.InvariantCultureIgnoreCase)));

            foreach (var field in fields)
            {
                var property = properties.FirstOrDefault(p => p.Name.Equals(field, StringComparison.InvariantCultureIgnoreCase));
                if (property != null)
                {
                    result.Add(property);
                }
            }

            return result;
        }

        private Cell ConstructCell(object value, CellValues dataType)
        {
            if (value is DateTimeOffset)
                value = ((DateTimeOffset)value).ToLocalTime().ToString("dd.MM.yyyy", CultureInfo.InvariantCulture);

            return new Cell
            {
                CellValue = new CellValue(value?.ToString()),
                DataType = new EnumValue<CellValues>(dataType)
            };
        }
    }
}