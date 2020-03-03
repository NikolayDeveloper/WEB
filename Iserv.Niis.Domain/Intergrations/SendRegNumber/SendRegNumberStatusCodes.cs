namespace Iserv.Niis.Domain.Intergrations.SendRegNumber
{
    /// <summary>
    /// Коды ответов метода отправки рег. номера в ЛК
    /// </summary>
    public class SendRegNumberStatusCodes
    {
        /// <summary>
        /// Успешно
        /// </summary>
        public const int Successfully = 1;

        /// <summary>
        /// Документ не найден
        /// </summary>
        public const int NotFound = 2;

        /// <summary>
        /// Тип заявки не совпадает
        /// </summary>
        public const int UnknownType = 3;

        /// <summary>
        /// Некорректные входные данные
        /// </summary>
        public const int BadRequest = 4;

        /// <summary>
        /// Документ уже был зарегистрирован
        /// </summary>
        public const int ExistNumber = 5;
    }
}