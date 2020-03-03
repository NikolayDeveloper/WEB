using System;

namespace Iserv.Niis.Documents.Attributes
{
    public class DocumentGeneratorAttribute : Attribute
    {
        /// <inheritdoc />
        /// <summary>
        ///     Метаданные для поиска шаблона на основании данных о типе документа
        /// </summary>
        /// <param name="documentTypeId">идентификатор типа документа в БД, константное значение</param>
        /// <param name="documentTypeCode">код типа документа в БД, константное значение</param>
        public DocumentGeneratorAttribute(int documentTypeId, string documentTypeCode)
        {
            DocumentTypeCode = documentTypeCode;
            DocumentTypeId = documentTypeId;
        }

        public int DocumentTypeId { get; }
        public string DocumentTypeCode { get; }
    }
}