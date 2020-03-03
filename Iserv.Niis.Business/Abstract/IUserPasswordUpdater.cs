using System.Threading.Tasks;
using Iserv.Niis.Domain.Entities.Security;

namespace Iserv.Niis.Business.Abstract
{
    /// <summary>
    /// Механизм обновления или установки пароля
    /// </summary>
    public interface IUserPasswordUpdater
    {
        /// <summary>
        /// Обновляет пароль пользователя
        /// </summary>
        /// <param name="user">Существующий пользователь</param>
        /// <param name="password">пароль для установки</param>
        /// <returns></returns>
        Task UpdateAsync(ApplicationUser user, string password);
    }
}
