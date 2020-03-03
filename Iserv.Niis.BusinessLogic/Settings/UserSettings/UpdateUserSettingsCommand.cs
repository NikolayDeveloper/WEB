using Iserv.Niis.Domain.Entities.Settings;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Commands;

namespace Iserv.Niis.BusinessLogic.Settings.UserSettings
{
    /// <summary>
    /// Обновляет настройки пользователя.
    /// </summary>
    public class UpdateUserSettingsCommand : BaseCommand
    {
        /// <summary>
        /// Выполнение запроса.
        /// </summary>
        /// <param name="userSetting">Пользовательские настройки.</param>
        public async void Execute(UserSetting userSetting)
        {
            var repository = Uow.GetRepository<UserSetting>();

            repository.Update(userSetting);

            Uow.SaveChanges();
        }
    }
}
