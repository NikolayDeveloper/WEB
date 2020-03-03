namespace Iserv.Niis.Domain.Intergrations
{
    /// <summary>
    /// Список событий SOAP запроса
    /// </summary>
    public class SoapActions
    {
        /// <summary>
        /// Отправка статуса заявки
        /// </summary>
        public const string SendStatus = "sendStatus";

        /// <summary>
        /// Отправка номера заявки
        /// </summary>
        public const string SendRegNumber = "sendRegNumber";

        /// <summary>
        /// Отправка документов переписки
        /// </summary>
        public const string SendMessage = "sendMessage";

        /// <summary>
        /// Отправка заявкав ЛК
        /// </summary>
        public const string RequisitionSend = "RequisitionSend";
    }
}