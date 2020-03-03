using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace Iserv.Niis.Integration.OneC.API.Classes
{
    /// <summary>
    /// <see cref="ObjectResult"/> выполняет согласование содержимого, форматирует тело объекта и создает
    /// ответ <see cref="StatusCodes.Status500InternalServerError"/> если согласование и форматирование завершены успешно.
    /// </summary>
    [DefaultStatusCode(StatusCodes.Status500InternalServerError)]
    public class InternalServerErrorObjectResult : ObjectResult
    {
        #region Конструктор
        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="value">Содержимое для форматирования в теле объекта.</param>
        public InternalServerErrorObjectResult(object value) : base(value)
        {
            StatusCode = StatusCodes.Status500InternalServerError;
        }
        #endregion
    }
}
