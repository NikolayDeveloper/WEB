using System;

namespace Iserv.Niis.Domain.Entities.Request
{
    /// <summary>
    /// Связь между заявкой и охранным документом.
    /// </summary>
    public class RequestProtectionDocSimilar
    {
        /// <summary>
        /// Дата создания.
        /// </summary>
        public DateTimeOffset DateCreate { get; set; }

        /// <summary>
        /// ID заявки.
        /// </summary>
        public int RequestId { get; set; }

        /// <summary>
        /// Заявка.
        /// </summary>
        public Request Request { get; set; }

        /// <summary>
        /// ID охранного документа.
        /// </summary>
        public int ProtectionDocId { get; set; }

        /// <summary>
        /// Охранный документ.
        /// </summary>
        public ProtectionDoc.ProtectionDoc ProtectionDoc { get; set; }
    }
}
