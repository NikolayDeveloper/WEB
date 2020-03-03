using System.Threading.Tasks;
using Iserv.Niis.Domain.Entities.Settings;
using Microsoft.EntityFrameworkCore;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Queries;

namespace Iserv.Niis.BusinessLogic.Settings.UserSettings
{
    /// <summary>
    /// Выборка пользовательских настроек.
    /// </summary>
    public class GetUserSettingsByUserIdAndKeyQuery : BaseQuery
    {
        /// <summary>
        /// Выполнение запроса.
        /// </summary>
        /// <param name="userId">ID пользователя.</param>
        /// <param name="key">Уникальный ключ настроек.</param>
        /// <returns></returns>
        public async Task<UserSetting> ExecuteAsync(int userId, string key)
        {
            var repo = Uow.GetRepository<UserSetting>().AsQueryable();

            return await repo.SingleOrDefaultAsync(us => us.UserId == userId && us.Key == key);
        }
    }
}
