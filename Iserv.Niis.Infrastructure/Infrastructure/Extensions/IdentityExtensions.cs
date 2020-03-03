using System.Security.Authentication;
using System.Security.Claims;
using System.Security.Principal;

namespace Iserv.Niis.Infrastructure.Extensions
{
    public static class IdentityExtensions
    {
        public static int GetUserId(this IIdentity identity)
        {
            var claim = ((ClaimsIdentity)identity).FindFirst("Id");

            if (claim == null)
            {
                throw new AuthenticationException("User not authenticated");
            }
            return int.Parse(claim.Value);
        }
    }
}
