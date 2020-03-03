using System.Threading.Tasks;
using Iserv.Niis.Domain.Entities.Security;

namespace Iserv.Niis.Business.Abstract
{
    /// <summary>
    /// Механизм обновления ролей пользователя
    /// </summary>
    public interface IUserRolesUpdater
    {
        /// <summary>
        /// Обновляет роли пользователя через RoleManager
        /// </summary>
        /// <param name="user">Существующий пользователь</param>
        /// <param name="roleIds">Список идентификаторов ролей</param>
        Task UpdateAsync(ApplicationUser user, params int[] roleIds);
    }
}