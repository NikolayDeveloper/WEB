using System;

namespace Iserv.Niis.Integration.ApiResult.Models
{
    /// <summary>
    /// Исключение которое произошло в API.
    /// </summary>
    public class ApiResultException
    {
        #region Свойства
        /// <summary>
        /// Тип исключения.
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Возвращает сообщение, описывающее текущее исключение.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Получает строковое представление непосредственных кадров в стеке вызова.
        /// </summary>
        public string StackTrace { get; set; }

        /// <summary>
        /// Возвращает или задает HRESULT — кодированное числовое значение, присвоенное определенному исключению.
        /// </summary>
        public int HResult { get; set; }

        /// <summary>
        /// Возвращает экземпляр класса <see cref="Exception"/>, который вызвал текущее исключение.
        /// </summary>
        public ApiResultException InnerException { get; set; }
        #endregion

        /// <summary>
        /// Получаем исключение которое произошло в API.
        /// </summary>
        /// <param name="exception">Представляет ошибки, происходящие во время выполнения приложения.</param>
        /// <returns>Исключение которое произошло в API.</returns>
        public static ApiResultException GetApiResultException(Exception exception)
        {
            if (exception == null)
                return null;

            var apiException = new ApiResultException
            {
                HResult = exception.HResult,
                Message = exception.Message,
                StackTrace = exception.StackTrace,
                Type = exception.GetType().ToString(),
                InnerException = GetApiResultException(exception.InnerException)
            };

            return apiException;
        }
    }
}
