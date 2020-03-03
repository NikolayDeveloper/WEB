using System.Threading.Tasks;
using Iserv.Niis.Domain.Entities.Security;

namespace Iserv.Niis.Business.Abstract
{
    /// <summary>
    /// Механизм обновления клэймов роли
    /// </summary>
    public interface IRoleClaimsUpdater
    {
        Task UpdateAsync(ApplicationRole role, string[] claims);
    }
}
