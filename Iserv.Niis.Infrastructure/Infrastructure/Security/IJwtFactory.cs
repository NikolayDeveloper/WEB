using System.Threading.Tasks;
using Iserv.Niis.Domain.Entities.Security;

namespace Iserv.Niis.Infrastructure.Security
{
    public interface IJwtFactory
    {
        /// <summary>
        /// Генерирует зашифрованный токен
        /// </summary>
        /// <returns></returns>
        Task<JwtTokenResponse> Create(ApplicationUser user);
    }

}
