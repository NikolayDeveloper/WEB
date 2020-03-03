using System.Threading.Tasks;
using Iserv.Niis.Business.Abstract;
using Iserv.Niis.Domain.Entities.Security;
using Microsoft.AspNetCore.Identity;

namespace Iserv.Niis.Business.Implementations
{
    public class UserPasswordUpdater : IUserPasswordUpdater
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public UserPasswordUpdater(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task UpdateAsync(ApplicationUser user, string password)
        {
            if (!string.IsNullOrEmpty(password))
            {
                await _userManager.RemovePasswordAsync(user);
                await _userManager.AddPasswordAsync(user, password.Trim());
            }
        }
    }
}
