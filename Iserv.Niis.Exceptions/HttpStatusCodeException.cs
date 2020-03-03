using System;
using System.Net;

namespace Iserv.Niis.Exceptions
{
    /// <summary>
    /// Представляет ошибки, происходящие во время выполнения приложения cодержащие значения кодов состояний, определенных для протокола HTTP.
    /// </summary>
    public class HttpStatusCodeException : Exception
    {
        /// <summary>
        /// Содержит значения кодов состояний, определенных для протокола HTTP.
        /// </summary>
        public HttpStatusCode HttpStatusCode { get; set; }

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="httpStatusCode">Содержит значения кодов состояний, определенных для протокола HTTP.</param>
        public HttpStatusCodeException(HttpStatusCode httpStatusCode)
        {
            HttpStatusCode = httpStatusCode;
        }

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="httpStatusCode">Содержит значения кодов состояний, определенных для протокола HTTP.</param>
        /// <param name="message">Возвращает сообщение, описывающее текущее исключение.</param>
        public HttpStatusCodeException(HttpStatusCode httpStatusCode, string message) : base(message)
        {
            HttpStatusCode = httpStatusCode;
        }

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="httpStatusCode">Содержит значения кодов состояний, определенных для протокола HTTP.</param>
        /// <param name="message">Возвращает сообщение, описывающее текущее исключение.</param>
        /// <param name="innerException">Возвращает экземпляр класса <see cref="Exception"/>, который вызвал текущее исключение.</param>
        public HttpStatusCodeException(HttpStatusCode httpStatusCode, string message, Exception innerException) : base(message, innerException)
        {
            HttpStatusCode = httpStatusCode;
        }
    }
}
