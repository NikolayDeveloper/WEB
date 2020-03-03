using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Iserv.Niis.DataBridge.Implementations;
using Iserv.Niis.Domain.Entities.Security;
using Microsoft.AspNetCore.Identity;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Commands;

namespace Iserv.Niis.BusinessLogic.Roles
{
    public class UpdateRoleClaimsCommand : BaseCommand
    {
        private readonly RoleManager<ApplicationRole> _roleManager;

        public UpdateRoleClaimsCommand(RoleManager<ApplicationRole> roleManager)
        {
            _roleManager = roleManager;
        }

        public void Execute(ApplicationRole role, string[] claims)
        {
            var repository = Uow.GetRepository<ClaimConstant>();

            var dtoClaims = repository.AsQueryable()
                .Where(x => claims.Contains(x.Value))
                .Select(x => x.Value)
                .ToList();

            var claimsRepository = Uow.GetRepository<IdentityRoleClaim<int>>();

            var currentClaims = claimsRepository.AsQueryable().Where(x => x.RoleId == role.Id);

            var claimsForAdd = dtoClaims
                .Except(currentClaims.Select(x => x.ClaimValue))
                .Select(x => new IdentityRoleClaim<int> { ClaimType = "prm", ClaimValue = x, RoleId = role.Id});

            var claimsForRemove = currentClaims
                .Where(x => !dtoClaims.Contains(x.ClaimValue))
                .ToList();

            claimsRepository.CreateRange(claimsForAdd);
            claimsRepository.DeleteRange(claimsForRemove);

            Uow.SaveChanges();
        }
    }
}
