using System.Security.Claims;
using Iserv.Niis.Authentication.UserIdentity;
using Microsoft.AspNetCore.Http;

namespace Iserv.Niis.Authentication
{
    public class NiisUserAuthenticationService : INiisUserAuthenticationService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public NiisUserAuthenticationService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public IUserIdentity Identity => new ClaimBasedUserIdentity(_httpContextAccessor.HttpContext.User.Identity as ClaimsIdentity);
    }
}
