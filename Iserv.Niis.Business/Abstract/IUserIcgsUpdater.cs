using System.Threading.Tasks;
using Iserv.Niis.Domain.Entities.Security;

namespace Iserv.Niis.Business.Abstract
{
    /// <summary>
    ///     Механизм обновления МКТУ пользователя
    /// </summary>
    public interface IUserIcgsUpdater
    {
        /// <summary>
        ///     Обновляет роли пользователя через RoleManager
        /// </summary>
        /// <param name="user">Существующий пользователь</param>
        /// <param name="icgsIds">Список идентификаторов МКТУ</param>
        Task UpdateAsync(ApplicationUser user, params int[] icgsIds);
    }
}
