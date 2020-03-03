using System.Collections.Generic;
using Iserv.Niis.FileConverter;

namespace Iserv.Niis.Services.Interfaces
{
    /// <summary>
    /// Сервис, для работы с документами.
    /// </summary>
    public interface IDocumentService
    {
        /// <summary>
        /// Генерирует документ из шаблона.
        /// </summary>
        /// <param name="documentTypeCode">Код типа документа. Коды документов: <see cref="Iserv.Niis.Common.Codes.DicDocumentTypeCodes"/> </param>
        /// <param name="documentParameters">Данные, которые используются для генерации документа.</param>
        /// <returns>Сгенерированный документ.</returns>
        GeneratedDocument GenerateDocument(string documentTypeCode, Dictionary<string, object> documentParameters);
    }
}
