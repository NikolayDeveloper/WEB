using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace Iserv.Niis.Authentication.UserIdentity
{
    public class ClaimBasedUserIdentity : IUserIdentity
    {
        private const string CommonNameClaimType = "http://schemas.xmlsoap.org/claims/CommonName";

        private readonly ClaimsIdentity _identity;

        public ClaimBasedUserIdentity(ClaimsIdentity identity)
        {
            if (identity == null || !identity.IsAuthenticated)
            {
                return;
            }

            _identity = identity;
            UserId = identity.GetUserId();
            UserXin = identity.GetClaimValue("xin");
            Name = identity.GetClaimValue("name");
            Email = identity.GetClaimValue(ClaimTypes.Email);
            CommonName = identity.GetClaimValue(CommonNameClaimType);
            IsAuthenticated = identity.IsAuthenticated;
        }

        public int UserId { get; }
        public string UserXin { get; }

        public string Name { get; }

        public string CommonName { get; }

        public string Email { get; }

        public bool IsInRole(string role)
        {
            return _identity.Claims.Any(r => r.Value.Equals(role));
        }

        public bool IsInRoles(List<string> roles)
        {
            return roles.Any(IsInRole);
        }

        public bool IsInRole(string[] roles)
        {
            return roles.Any(IsInRole);
        }

        public bool IsAuthenticated { get; set; }

        public override string ToString()
        {
            return CommonName ?? Name;
        }
    }
}