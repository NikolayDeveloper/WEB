using System;

namespace Iserv.Niis.Integration.ApiResult.Models
{
    /// <summary>
    /// Базовый результат выполнения API.
    /// </summary>
    public class ApiResultBase
    {
        #region Конструктор
        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="exception">Представляет ошибки, происходящие во время выполнения приложения.</param>
        public ApiResultBase(ApiResultException exception)
        {
            Exception = exception;
        }
        #endregion

        #region Свойства
        /// <summary>
        /// Успешно ли выполнено действие.
        /// </summary>
        public bool IsSuccess => Exception == null;

        /// <summary>
        /// Представляет ошибки, происходящие во время выполнения приложения.
        /// </summary>
        public ApiResultException Exception { get; set; }
        #endregion
    }
}
