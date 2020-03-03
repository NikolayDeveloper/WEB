using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using Iserv.Niis.Domain.Constants;
using Newtonsoft.Json;

namespace Iserv.Niis.Infrastructure.Security
{
    public class JwtTokenResponse
    {
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }

        public object Profile { get; }

        public JwtTokenResponse(JwtToken token, ClaimsIdentity identity)
        {
            AccessToken = token.Value;
            Profile = new
            {
                id = identity.Claims.Single(c => c.Type == KeyFor.JwtClaimIdentifier.Id).Value,
                email = identity.Claims.Single(c => c.Type == JwtRegisteredClaimNames.Email).Value,
                name = identity.Claims.Single(c => c.Type == JwtRegisteredClaimNames.UniqueName).Value,
                prm = identity.Claims
                    .Where(x => x.Type == KeyFor.JwtClaimIdentifier.Permission)
                    .Select(x => x.Value),
                roles = identity.Claims.Where(x => x.Type == ClaimTypes.Role)
                    .Select(x => x.Value)
            };
        }
    }
}