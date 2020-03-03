using Iserv.Niis.BusinessLogic.Settings;
using Iserv.Niis.Domain.Enums;
using Microsoft.AspNetCore.Mvc;

namespace Iserv.Niis.Api.Controllers.Administration
{
    [Produces("application/json")]
    [Route("api/Settings")]
    public class SettingsController : BaseNiisApiController
    {
        /// <summary>
        /// Выборка системной константы по ее типу
        /// </summary>
        /// <param name="type">Тип константы</param>
        /// <returns></returns>
        [HttpGet("{type}")]
        public IActionResult GetSettingValueByType(SettingType type)
        {
            var result = Executor.GetQuery<GetSettingsByTypeQuery>().Process(q => q.Execute(type));

            return Ok(result?.Value);
        }
    }
}