using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Iserv.Niis.Business.Abstract;
using Iserv.Niis.DataAccess.EntityFramework;
using Iserv.Niis.Domain.Entities.Security;
using Microsoft.AspNetCore.Identity;

namespace Iserv.Niis.Business.Implementations
{
    public class RoleClaimsUpdater : IRoleClaimsUpdater
    {
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly NiisWebContext _context;

        public RoleClaimsUpdater(
            RoleManager<ApplicationRole> roleManager,
            NiisWebContext context)
        {
            _roleManager = roleManager;
            _context = context;
        }

        public async Task UpdateAsync(ApplicationRole role, string[] claims)
        {
            var dtoClaims = _context.ClaimConstants
                .Where(x => claims.Contains(x.Value))
                .Select(x => x.Value)
                .ToList();

            var currentClaims = await _roleManager.GetClaimsAsync(role);

            var claimsForAdd = dtoClaims
                .Except(currentClaims.Select(x => x.Value))
                .Select(x => new Claim("prm", x));

            var claimsForRemove = currentClaims
                .Where(x => !dtoClaims.Contains(x.Value))
                .ToList();

            foreach (var claim in claimsForAdd)
            {
                await _roleManager.AddClaimAsync(role, claim);
            }

            foreach (var claim in claimsForRemove)
            {
                await _roleManager.RemoveClaimAsync(role, claim);
            }
        }
    }
}
