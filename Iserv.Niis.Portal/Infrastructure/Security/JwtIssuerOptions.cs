using System;
using Microsoft.IdentityModel.Tokens;

namespace Iserv.Niis.Portal.Infrastructure.Security
{
    /// <summary>
    /// https://fullstackmark.com/post/10/user-authentication-with-angular-and-asp-net-core
    /// </summary>
    public class JwtIssuerOptions
    {
        public string Issuer { get; set; }

        public string Audience { get; set; }

        public string IssuerSigningKey { get; set; }
        public int ExpirityInMinutes { get; set; }
    }
}
