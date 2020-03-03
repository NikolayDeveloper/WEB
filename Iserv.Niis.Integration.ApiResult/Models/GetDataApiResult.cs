using System;

namespace Iserv.Niis.Integration.ApiResult.Models
{
    /// <summary>
    /// Результат выполнения API для получения данных.
    /// </summary>
    public sealed class GetDataApiResult<TData> : ApiResultBase
    {
        #region Конструктор
        /// <summary>
        /// Конструктор.
        /// </summary>
        public GetDataApiResult() : base(null)
        {

        }

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="data">Данные.</param>
        public GetDataApiResult(TData data) : base(null)
        {
            Data = data;
        }

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="exception">Представляет ошибки, происходящие во время выполнения приложения.</param>
        public GetDataApiResult(ApiResultException exception) : base(exception)
        {
            Data = default(TData);
        }
        #endregion

        #region Свойства
        /// <summary>
        /// Данные.
        /// </summary>
        public TData Data { get; set; }
        #endregion

        #region public методы
        /// <summary>
        /// Получить успешный результат.
        /// </summary>
        /// <param name="data">Данные.</param>
        public static GetDataApiResult<TData> GetSuccessResult(TData data)
        {
            return new GetDataApiResult<TData>(data);
        }

        /// <summary>
        /// Получить не успешный результат.
        /// </summary>
        /// <param name="resultException">Представляет ошибки, происходящие во время выполнения приложения.</param>
        public static GetDataApiResult<TData> GetErrorResult(ApiResultException resultException)
        {
            if (resultException == null)
                throw new ArgumentNullException(nameof(resultException));

            return new GetDataApiResult<TData>(resultException);
        }

        /// <summary>
        /// Получить не успешный результат.
        /// </summary>
        /// <param name="exception">Представляет ошибки, происходящие во время выполнения приложения.</param>
        public static GetDataApiResult<TData> GetErrorResult(Exception exception)
        {
            return GetErrorResult(ApiResultException.GetApiResultException(exception));
        }
        #endregion
    }
}
