namespace Iserv.Niis.Common.Codes
{
    public class DicDocumentClassificationCodes
    {
        /// <summary>
        /// Входящая корреспонденция.
        /// </summary>
        public const string Incoming = "01";

        /// <summary>
        /// Материалы заявок.
        /// </summary>
        public const string RequestMaterialsIncoming = "01.01";

        /// <summary>
        /// Заявление на Коммерциализацию
        /// </summary>
        public const string CommercializationApplication = "01.02";

        /// <summary>
        /// Исходящая корреспонденция.
        /// </summary>
        public const string Outgoing = "02";

        /// <summary>
        /// Материалы заявок.
        /// </summary>
        public const string RequestMaterialsOutgoing = "02.01";

        /// <summary>
        /// Внутренняя корреспонденция.
        /// </summary>
        public const string Internal = "03";

        /// <summary>
        /// Неиспользуемые.
        /// </summary>
        public const string Unused = "04";

        /// <summary>
        /// Документы заявки.
        /// </summary>
        public const string DocumentRequest = "05";
    }
}