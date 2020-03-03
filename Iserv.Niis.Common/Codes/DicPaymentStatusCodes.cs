namespace Iserv.Niis.Common.Codes
{
    public static class DicPaymentStatusCodes
    {
        /// <summary>
        /// Не оплачено
        /// </summary>
        public const string Notpaid = "notpaid";
        /// <summary>
        /// Зачтено
        /// </summary>
        public const string Credited = "credited";
        /// <summary>
        /// Списано
        /// </summary>
        public const string Charged = "charged";
        /// <summary>
        /// Распределённые – Сумма платежа полностью распределена
        /// </summary>
        public const string Distributed = "Distributed";
        /// <summary>
        /// Не распределённые – Сумма платежа частично распределена, либо не распределена
        /// </summary>
        public const string NotDistributed = "NotDistributed";
        /// <summary>
        /// Возвращено – Выполнен возврат платежа
        /// </summary>
        public const string Returned = "Returned";             
    }
}
