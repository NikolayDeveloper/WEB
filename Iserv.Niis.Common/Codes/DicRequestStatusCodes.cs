using System;
using System.Collections.Generic;
using System.Text;

namespace Iserv.Niis.Common.Codes
{
    /// <summary>
    /// Коды статусов заявок
    /// </summary>
    public class DicRequestStatusCodes
    {
        /// <summary>
        /// Отозвана по просьбе заявителя
        /// </summary>
        public const string RecalByRequest = "RecalByRequest";
        /// <summary>
        /// Отозвана в связи с неуплатой за полную экспертизу
        /// </summary>
        public const string RecallByNotPaid = "RecallByNotPaid";
        /// <summary>
        /// Отозвана в связи с не предоставление ответа на запрос
        /// </summary>
        public const string RecallByNotAnswered = "RecallByNotAnswered";
    }
}
