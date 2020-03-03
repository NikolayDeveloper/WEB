using System;
using Iserv.Niis.Integration.ApiResult;
using Iserv.Niis.Integration.ApiResult.Models;
using Iserv.Niis.Integration.OneC.API.Classes;
using Iserv.Niis.Integration.OneC.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; ;

namespace Iserv.Niis.Integration.OneC.API.Controllers
{
    /// <summary>
    /// Базовый контроллер.
    /// </summary>
    public class BaseApiController : ControllerBase
    {
        #region Локальные переменные
        /// <summary>
        /// Представляет набор команд возвращающих команды / запросы для выполнения.
        /// </summary>
        private IExecutor _executor;
        #endregion

        #region Свойства
        /// <summary>
        /// Представляет набор команд возвращающих команды / запросы для выполнения.
        /// </summary>
        protected IExecutor Executor => _executor ?? (_executor = NiisOneCIntegrationAmbientContext.Current.Executor);
        #endregion

        #region protected методы.
        /// <summary>
        /// Создает объект <see cref="OkObjectResult" /> который создает ответ <see cref="StatusCodes.Status200OK" />.
        /// </summary>
        /// <param name="value">Значение содержимого для форматирования в теле объекта.</param>
        /// <returns>Созданный <see cref="OkObjectResult" /> для ответа.</returns>
        protected OkObjectResult Ok<TData>(TData value)
        {
            return base.Ok(GetDataApiResult<TData>.GetSuccessResult(value));
        }
        /// <summary>
        /// Создает объект <see cref="NotFoundObjectResult" /> который создает ответ <see cref="StatusCodes.Status404NotFound" />.
        /// </summary>
        /// <param name="value">Значение содержимого для форматирования в теле объекта.</param>
        /// <returns>Созданный <see cref="OkObjectResult" /> для ответа.</returns>
        protected NotFoundObjectResult NotFound<TData>(TData value)
        {
            return base.NotFound(GetDataApiResult<TData>.GetSuccessResult(value));
        }

        /// <summary>
        /// Создает объект <see cref="InternalServerErrorObjectResult" /> который создает ответ <see cref="StatusCodes.Status500InternalServerError" />.
        /// </summary>
        /// <param name="exception">Представляет ошибки, происходящие во время выполнения приложения.</param>
        /// <returns>Созданный <see cref="InternalServerErrorObjectResult" /> для ответа.</returns>
        protected InternalServerErrorObjectResult InternalServerError<TData>(Exception exception)
        {
            return new InternalServerErrorObjectResult(GetDataApiResult<TData>.GetErrorResult(exception));
        }
        #endregion
    }
}
