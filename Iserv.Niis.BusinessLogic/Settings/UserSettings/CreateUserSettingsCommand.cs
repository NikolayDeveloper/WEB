using System.Threading.Tasks;
using Iserv.Niis.Domain.Entities.Settings;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Commands;

namespace Iserv.Niis.BusinessLogic.Settings.UserSettings
{
    /// <summary>
    /// Создание настроек пользователя.
    /// </summary>
    public class CreateUserSettingsCommand : BaseCommand
    {
        /// <summary>
        /// Выполнение запроса.
        /// </summary>
        /// <param name="userId">ID пользователя.</param>
        /// <param name="key">Уникальный ключ настроек.</param>
        /// <param name="value">Настройки.</param>
        public async Task ExecuteAsync(int userId, string key, string value)
        {
            var repository = Uow.GetRepository<UserSetting>();

            var userSetting = new UserSetting {UserId = userId, Key = key, Value = value};

            await repository.CreateAsync(userSetting);

            Uow.SaveChanges();
        }
    }
}
