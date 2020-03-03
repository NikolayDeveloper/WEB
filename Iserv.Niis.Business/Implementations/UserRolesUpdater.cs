using System.Linq;
using System.Threading.Tasks;
using Iserv.Niis.Business.Abstract;
using Iserv.Niis.Domain.Entities.Security;
using Microsoft.AspNetCore.Identity;

namespace Iserv.Niis.Business.Implementations
{
    public class UserRolesUpdater : IUserRolesUpdater
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;

        public UserRolesUpdater(RoleManager<ApplicationRole> roleManager,
            UserManager<ApplicationUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public async Task UpdateAsync(ApplicationUser user, params int[] roleIds)
        {
            var currentUserRoles = await _userManager.GetRolesAsync(user);
            var rolesByIds = _roleManager.Roles.Where(x => roleIds.Contains(x.Id)).Select(x => x.Name);

            var rolesForRemove = currentUserRoles.Except(rolesByIds).ToList();
            var rolesForAdd = rolesByIds.Except(currentUserRoles).ToList();

            await _userManager.RemoveFromRolesAsync(user, rolesForRemove);
            await _userManager.AddToRolesAsync(user, rolesForAdd);
        }
    }
}
