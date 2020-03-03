using Iserv.Niis.Domain.Entities.Security;
using Iserv.Niis.Migration.BusinessLogic.Abstract;
using Iserv.Niis.Migration.BusinessLogic.Services.NewNiis;
using Iserv.Niis.Migration.BusinessLogic.Services.OldNiis;
using Iserv.OldNiis.DataAccess.EntityFramework;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace Iserv.Niis.Migration.BusinessLogic.InsertOldData
{
    public class InsertSecurityUsersHandler : BaseHandler
    {
        private readonly NewNiisSecurityUserService _newNiisSecurityUserService;
        private readonly OldNiisSecurityUserService _oldNiisSecurityUserService;

        public InsertSecurityUsersHandler(
            NewNiisSecurityUserService newNiisSecurityUserService,
            OldNiisSecurityUserService oldNiisSecurityUserService,
            NiisWebContextMigration context) : base(context)
        {
            _newNiisSecurityUserService = newNiisSecurityUserService;
            _oldNiisSecurityUserService = oldNiisSecurityUserService;
        }

        public void Execute()
        {
            MigrateSecurityUsers(typeof(ApplicationUser), _oldNiisSecurityUserService.GetApplicationUsers);

            MigrateSecurityUsers(typeof(ApplicationRole), _oldNiisSecurityUserService.GetApplicationRoles);

            MigrateSecurityUsers(typeof(IdentityUserRole<int>), _oldNiisSecurityUserService.GetIdentityUserRoles);
        }

        #region Private Methods

        private void MigrateSecurityUsers(Type entityType, Func<IEnumerable<object>> getOldData)
        {
            var isAnySecurityUsers = _newNiisSecurityUserService.IsAnySecurityUsers(entityType);
            if (isAnySecurityUsers == false)
            {
                var securityUsers = getOldData();
                ActionTransaction(() => _newNiisSecurityUserService.CreateRangeSecurityUsers(securityUsers, entityType));
            }
        }

        #endregion
    }
}
