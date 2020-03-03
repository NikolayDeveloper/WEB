using System;
using System.Threading.Tasks;
using Iserv.Niis.Domain.Entities.Security;
using Microsoft.AspNetCore.Identity;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Commands;

namespace Iserv.Niis.BusinessLogic.Security
{
    public class CreateUserCommand : BaseCommand
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public CreateUserCommand(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }
        public void Execute(ApplicationUser user)
        {
            _userManager.CreateAsync(user, user.Password).Wait();
            _userManager.SetLockoutEnabledAsync(user, false).Wait();
        }
    }
}