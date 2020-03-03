using System;
using System.Linq;
using Iserv.Niis.Common.Codes;
using Iserv.Niis.Domain.Entities.Security;
using Microsoft.AspNetCore.Identity;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Commands;

namespace Iserv.Niis.BusinessLogic.Security
{
    public class UpdateUserRolesCommand : BaseCommand
    {
        public void Execute(int userId, int[] roleIds)
        {
            var repo = Uow.GetRepository<IdentityUserRole<int>>();
            var userRepository = Uow.GetRepository<ApplicationUser>();
            var roleRepository = Uow.GetRepository<ApplicationRole>();

            var user = userRepository.
                AsQueryable().
                FirstOrDefault(u=>u.Id == userId);

            var transferredFiredMaternityUserRole = roleRepository.
                AsQueryable()
                .FirstOrDefault(r => r.Code == RoleCodes.TransferredFiredMaternity);
            
            var oldUserRoles = repo
                .AsQueryable()
                .Where(ur => ur.UserId == userId)
                .ToList();

            

            var rolesForRemove = oldUserRoles.Where(ur => !roleIds.Contains(ur.RoleId));
            var rolesForAdd = roleIds.Except(oldUserRoles.Select(ur => ur.RoleId))
                .Select(roleId => new IdentityUserRole<int> { UserId = userId, RoleId = roleId }).ToList();

            if (transferredFiredMaternityUserRole != null)
            {
                if (rolesForRemove
                    .Any(rfr => rfr.UserId == user.Id && 
                                rfr.RoleId == transferredFiredMaternityUserRole.Id))
                {
                    user.IsDeleted = false;
                    user.DeletedDate = null;
                    userRepository.Update(user);
                }

                if(rolesForAdd
                    .Any(rfa => rfa.UserId == user.Id && 
                                rfa.RoleId == transferredFiredMaternityUserRole.Id))
                {
                    user.IsDeleted = true;
                    user.DeletedDate = DateTimeOffset.Now;
                    userRepository.Update(user);
                }
            }

            repo.DeleteRange(rolesForRemove);
            repo.CreateRange(rolesForAdd);

            Uow.SaveChanges();
        }
    }
}