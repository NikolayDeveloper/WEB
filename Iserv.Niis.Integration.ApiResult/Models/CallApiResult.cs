using System;

namespace Iserv.Niis.Integration.ApiResult.Models
{
    /// <summary>
    /// Результат выполнения API.
    /// </summary>
    public sealed class CallApiResult : ApiResultBase
    {
        #region Конструктор
        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="exception">Представляет ошибки, происходящие во время выполнения приложения.</param>
        public CallApiResult(ApiResultException exception) : base(exception)
        {
        }
        #endregion

        #region public методы
        /// <summary>
        /// Получить не успешный результат.
        /// </summary>
        /// <param name="resultException">Представляет ошибки, происходящие во время выполнения приложения.</param>
        public static CallApiResult GetErrorResult(ApiResultException resultException)
        {
            if (resultException == null)
                throw new ArgumentNullException(nameof(resultException));

            return new CallApiResult(resultException);
        }

        /// <summary>
        /// Получить не успешный результат.
        /// </summary>
        /// <param name="exception">Представляет ошибки, происходящие во время выполнения приложения.</param>
        public static CallApiResult GetErrorResult(Exception exception)
        {
            return GetErrorResult(ApiResultException.GetApiResultException(exception));
        }
        #endregion
    }
}
