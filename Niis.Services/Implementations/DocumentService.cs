using System;
using System.Collections.Generic;
using Iserv.Niis.Documents.Abstractions;
using Iserv.Niis.FileConverter;
using Iserv.Niis.Services.Interfaces;

namespace Iserv.Niis.Services.Implementations
{
    /// <summary>
    /// Сервис, для работы с документами.
    /// </summary>
    public class DocumentService : IDocumentService
    {
        private readonly IDocumentGeneratorFactory _documentGeneratorFactory;

        public DocumentService(IDocumentGeneratorFactory documentGeneratorFactory)
        {
            _documentGeneratorFactory = documentGeneratorFactory;
        }

        /// <summary>
        /// Генерирует документ из шаблона.
        /// </summary>
        /// <param name="documentTypeCode">Код типа документа. Коды документов: <see cref="Iserv.Niis.Common.Codes.DicDocumentTypeCodes"/> </param>
        /// <param name="documentParameters">Данные, которые используются для генерации документа.</param>
        /// <returns>Сгенерированный документ.</returns>
        public GeneratedDocument GenerateDocument(string documentTypeCode, Dictionary<string, object> documentParameters)
        {
            var documentGenerator = _documentGeneratorFactory.Create(documentTypeCode);

            if (documentGenerator is null)
            {
                throw new ArgumentException("В системе нет генератора документа для данного типа документа.", nameof(documentTypeCode));
            }

            return documentGenerator.Process(documentParameters);
        }
    }
}
