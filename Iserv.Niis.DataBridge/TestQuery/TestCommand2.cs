using Iserv.Niis.DataBridge.Implementations;
using Iserv.Niis.Domain.Entities.Security;
//using Iserv.Niis.Model.Models.User;
using Microsoft.AspNetCore.Identity;
//using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Commands;
using System.Linq;

namespace Iserv.Niis.DataBridge.TestQuery
{
    public class TestCommand2 : BaseCommand
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public TestCommand2(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }
        public void Execute(int Id)
        {
            var user = _userManager.Users.Where(u => u.Id == Id).FirstOrDefault();
        }
    }
}
