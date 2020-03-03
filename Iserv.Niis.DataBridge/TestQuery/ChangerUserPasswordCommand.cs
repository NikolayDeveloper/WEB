using Iserv.Niis.DataBridge.Implementations;
using Iserv.Niis.Domain.Entities.Security;
//using Iserv.Niis.Model.Models.User;
using Microsoft.AspNetCore.Identity;
//using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Commands;
using System.Linq;

namespace Iserv.Niis.DataBridge.TestQuery
{
   // public class ChangerUserPasswordCommand1 : BaseCommand
   // {
   //     private readonly UserManager<ApplicationUser> _userManager;

   //     public ChangerUserPasswordCommand1(UserManager<ApplicationUser> userManager)
   //     {
   //         _userManager = userManager;
   //     }
   //     public void Execute(UserDetailsDto userDetailsDto)
   //     {
			//if (string.IsNullOrEmpty(userDetailsDto.Password)) return;

   //         var user = _userManager.Users.Where(u => u.Id == userDetailsDto.Id).FirstOrDefault();

   //         if (user.Password == userDetailsDto.Password) return;

   //         _userManager.ChangePasswordAsync(user, user.Password, userDetailsDto.Password).Wait();
   //         user.PasswordHash = _userManager.PasswordHasher.HashPassword(user, user.Password);

   //         _userManager.UpdateAsync(user).Wait();
   //     }
   // }
}
