namespace Iserv.Niis.Domain.Intergrations
{
    /// <summary>
    /// Коды ответов сервера ЛК
    /// </summary>
    public class ServerStatusCodes
    {
        /// <summary>
        /// Успешно
        /// </summary>
        public const int Successfully = 1;

        /// <summary>
        /// Тип заявки не совпадает
        /// </summary>
        public const int UnknownType = 2;

        /// <summary>
        /// Некорректные входные данные
        /// </summary>
        public const int BadRequest = 3;
    }
}