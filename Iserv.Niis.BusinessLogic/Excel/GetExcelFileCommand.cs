using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Iserv.Niis.Business.Abstract;
using Microsoft.AspNetCore.Http;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Commands;

namespace Iserv.Niis.BusinessLogic.Excel
{
    /// <summary>
    /// Команда получающая Excel файл.
    /// </summary>
    public class GetExcelFileCommand : BaseCommand
    {
        private readonly IFileExporter _fileExporter;
        private const string ExcelfieldsKey = "excelFields";
        public const string ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        public const string DefaultFileName = "file.xlsx";

        public GetExcelFileCommand(IFileExporter fileExporter)
        {
            _fileExporter = fileExporter;
        }

        /// <summary>
        /// Выполнение запроса
        /// </summary>
        /// <typeparam name="T">Тип данных.</typeparam>
        /// <param name="data">Коллекция даных.</param>
        /// <param name="httpRequest">Запрос.</param>
        /// <returns></returns>
        public MemoryStream Execute<T>(IEnumerable<T> data, HttpRequest httpRequest)
        {
            var fileStream = _fileExporter.Export(data, FileType.Xlsx, GetExcelFields(httpRequest));
            return fileStream;
        }

        /// <summary>
        /// Получает поля которые должны быть выведены в Excel.
        /// </summary>
        /// <param name="httpRequest">Запрос.</param>
        /// <returns>Список полей которые должны быть в Excel.</returns>
        private string[] GetExcelFields(HttpRequest httpRequest)
        {
            try
            {
                if (!httpRequest.Query.TryGetValue(ExcelfieldsKey, out var values) || values.Count <= 0)
                {
                    return new string[0];
                }

                if (values.Count > 1)
                {
                    throw new Exception($"Allowed only one {ExcelfieldsKey} parameter");
                }

                return values.Single().Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                throw new Exception("Excel export error! See inner exception for details.", e);
            }
        }
    }
}
