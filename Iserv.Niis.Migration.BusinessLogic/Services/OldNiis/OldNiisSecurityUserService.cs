using Iserv.Niis.Domain.Entities.Security;
using Iserv.Niis.Migration.BusinessLogic.Abstract;
using Iserv.Niis.Migration.BusinessLogic.Utils;
using Iserv.OldNiis.DataAccess.EntityFramework;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Iserv.Niis.Migration.BusinessLogic.Services.OldNiis
{
    public class OldNiisSecurityUserService : BaseService
    {
        private readonly static string DefaultPassword = "123456";

        private readonly OldNiisContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public OldNiisSecurityUserService(
            OldNiisContext context,
            UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }


        public List<ApplicationUser> GetApplicationUsers()
        {
            var users = _context.Users
                .AsNoTracking()
                .ToList();

            var applicatonUsers = users.Select(u => new ApplicationUser
            {
                Id = u.Id,
                DateCreate = u.DateCreate ?? DateTimeOffset.Now,
                DateUpdate = u.DateChange ?? DateTimeOffset.Now,
                DepartmentId = u.DepartmentId,
                PositionId = u.PositionId,
                NameRu = u.NameRu,
                NameEn = u.NameEn,
                NameKz = u.NameKz,
                XIN = u.IIN,
                UserName = u.IIN,
                Email = u.Email,
                IpAddress = u.IpAddress,
                IsArchive = CustomConverter.StringToNullableBool(u.IsArchive),
                IsVirtual = CustomConverter.StringToBool(u.Virtual),
                Password = string.IsNullOrWhiteSpace(u.Password) == false ? u.Password : DefaultPassword,
                TemplateUserName = u.TemplateUserName,
                SecurityStamp = Guid.NewGuid().ToString()
            }).ToList();

            SetHashPasswordOnApplicationUsers(applicatonUsers);

            return applicatonUsers;
        }

        public List<ApplicationRole> GetApplicationRoles()
        {
            var roles = _context.Groups
                .AsNoTracking()
                .ToList();

            return roles.Select(r => new ApplicationRole
            {
                Id = r.Id,
                Code = r.Code,
                Name = r.Code,
                NormalizedName = r.Code,
                ConcurrencyStamp = Guid.NewGuid().ToString(),
                DateCreate = DateTimeOffset.Now,
                DateUpdate = DateTimeOffset.Now,
                NameRu = r.NameRu,
                NameEn = r.NameEn,
                NameKz = r.NameKz, 
            }).ToList();
        }

        public List<IdentityUserRole<int>> GetIdentityUserRoles()
        {
            var userRoles = _context.Users
                .AsNoTracking()
                .Where(u => u.GroupId.HasValue)
                .Select(u => new IdentityUserRole<int>
                {
                    RoleId = u.GroupId.Value,
                    UserId = u.Id
                }).ToList();

            return userRoles;
        }

        #region Private Methods

        private void SetHashPasswordOnApplicationUsers(List<ApplicationUser> users)
        {
            users.ForEach(user =>
            {
                user.PasswordHash = _userManager.PasswordHasher.HashPassword(user, user.Password);
            });
        }

        #endregion


    }
}
