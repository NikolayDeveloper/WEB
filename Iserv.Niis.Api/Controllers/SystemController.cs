using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Reflection;
using Iserv.Niis.BusinessLogic.Settings.UserSettings;
using Iserv.Niis.DI;
using Iserv.Niis.Model.Models.System;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; ;

namespace Iserv.Niis.Api.Controllers
{
    /// <summary>
    /// Системный контроллер.
    /// </summary>
    [Route("api/System")]
    public class SystemController : BaseNiisApiController
    {
        /// <summary>
        /// Интерфейс для выполнения запросов.
        /// </summary>
        private readonly IExecutor _executor;

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="executor">Интерфейс для выполнения запросов.</param>
        public SystemController(IExecutor executor)
        {
            _executor = executor;
        }

        /// <summary>
        /// Возвращает текущую версию сборки.
        /// </summary>
        /// <returns>Текущая версия сборки.</returns>
        [HttpGet("Version")]
        [AllowAnonymous]
        public async Task<IActionResult> Version()
        {
            Version version = Assembly.GetExecutingAssembly().GetName().Version;

            var result = new
            {
                version.Major,
                version.Minor,
                version.Build,
                version.Revision,
                date = new DateTime(2000, 1, 1).Add(new TimeSpan(TimeSpan.TicksPerDay * version.Build + TimeSpan.TicksPerSecond * 2 * version.Revision))
            };

            return Ok(result);
        }

        /// <summary>
        /// Загружает пользовательские настройки.
        /// </summary>
        /// <param name="key">Ключ к которому относятся настройки.</param>
        /// <returns></returns>
        [HttpGet("LoadUserSettings/{key}")]
        public async Task<IActionResult> LoadUserSettings(string key)
        {
            var userId = NiisAmbientContext.Current.User.Identity.UserId;

            var userSetting = await _executor.GetQuery<GetUserSettingsByUserIdAndKeyQuery>()
                .Process(q => q.ExecuteAsync(userId, key));

            return Ok(userSetting?.Value);
        }

        /// <summary>
        /// Сохраняет пользовательские настройки.
        /// </summary>
        /// <param name="userSettingsDto">Пользовательские настройки.</param>
        /// <returns></returns>
        [HttpPost("SaveUserSettings")]
        public async Task<IActionResult> SaveUserSettings([FromBody] UserSettingsDto userSettingsDto)
        {
            var userId = NiisAmbientContext.Current.User.Identity.UserId;

            var userSetting = await _executor.GetQuery<GetUserSettingsByUserIdAndKeyQuery>()
                .Process(q => q.ExecuteAsync(userId, userSettingsDto.Key));

            if (userSetting == null)
            {
                await _executor.GetCommand<CreateUserSettingsCommand>()
                    .Process(q => q.ExecuteAsync(userId, userSettingsDto.Key, userSettingsDto.Value));
            }
            else
            {
                userSetting.Value = userSettingsDto.Value;
                _executor.GetCommand<UpdateUserSettingsCommand>().Process(q => q.Execute(userSetting));
            }

            return Ok();
        }
    }
}
