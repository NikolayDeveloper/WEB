using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Iserv.Niis.Domain.Constants;
using Iserv.Niis.Domain.Entities.Security;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;

namespace Iserv.Niis.Portal.Infrastructure.Security
{
    public class JwtFactory : IJwtFactory
    {
        private readonly IConfiguration _configuration;
        private readonly  SignInManager<ApplicationUser> _signInManager;

        public JwtFactory(IConfiguration configuration, SignInManager<ApplicationUser> signInManager)
        {
            _configuration = configuration;
            _signInManager = signInManager;
        }
        
        public async Task<JwtTokenResponse> Create(ApplicationUser user)
        {

            var claimsPrincipal = await _signInManager.CreateUserPrincipalAsync(user);

            var identity = new ClaimsIdentity(claimsPrincipal.Claims.Distinct(), "Token");
            var prms = identity.Claims
                .Where(x => x.Type == KeyFor.JwtClaimIdentifier.Permission)
                .Select(x => x.Value)
                .ToList();
            var roles = identity.Claims
                .Where(x => x.Type == ClaimTypes.Role)
                .Select(x => x.Value).ToList();

            var tokenBuilder = new JwtTokenBuilder()
                .WithDefaultOptions(_configuration)
                .AddSubject(user.UserName)
                .AddClaim("id", identity.Claims.Single(c => c.Type == KeyFor.JwtClaimIdentifier.Id).Value)
                .AddClaim("email", identity.Claims.Single(c => c.Type == JwtRegisteredClaimNames.Email).Value)
                .AddClaim("name", identity.Claims.Single(c => c.Type == JwtRegisteredClaimNames.UniqueName).Value);

            foreach (var prm in prms)
                tokenBuilder.AddClaim(KeyFor.JwtClaimIdentifier.Permission, prm);
            foreach (var prm in roles)
                tokenBuilder.AddClaim(KeyFor.JwtClaimIdentifier.Role, prm);

            var token = tokenBuilder.Build();

            return await Task.FromResult(new JwtTokenResponse(token, identity));
        }
    }
}