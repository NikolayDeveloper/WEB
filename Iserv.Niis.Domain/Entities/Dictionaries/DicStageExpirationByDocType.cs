using Iserv.Niis.Domain.Abstract;
using Iserv.Niis.Domain.Enums;

namespace Iserv.Niis.Domain.Entities.Dictionaries
{
    public class DicStageExpirationByDocType : DictionaryEntity<int>, IHaveConcurrencyToken
    {
        /// <summary>
        /// Этап
        /// </summary>
        public DicRouteStage RouteStage { get; set; }
        public int RouteStageId { get; set; }

        /// <summary>
        /// Тип документа
        /// </summary>
        public DicDocumentType DocumentType { get; set; }
        public int DocumentTypeId { get; set; }

        /// <summary>
        /// Срок исполнения
        /// </summary>
        public ExpirationType ExpirationType { get; set; }
        public short ExpirationValue { get; set; }
    }
}
