using System;
using System.Linq;
using System.Security.Claims;

namespace Iserv.Niis.Authentication.UserIdentity
{
    public static class ClaimsIdentityExtensions
    {
        public static string GetClaimValue(this ClaimsIdentity identity, string key)
        {
            return identity.Claims?.FirstOrDefault(e => e.Type == key)?.Value;
        }

        public static int GetUserId(this ClaimsIdentity identity)
        {
            return int.Parse(identity.Claims?.FirstOrDefault(e => e.Type == "id")?.Value ?? throw new InvalidOperationException(nameof(GetUserId)));
        }
    }
}