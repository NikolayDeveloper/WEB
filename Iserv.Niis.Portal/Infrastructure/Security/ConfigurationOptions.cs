using System;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;

namespace Iserv.Niis.Portal.Infrastructure.Security
{
    public static class ConfigurationOptions
    {
        public static void IdentityOptions(IdentityOptions options)
        {
            // Password settings
            options.Password.RequireDigit = false;
            options.Password.RequireLowercase = false;
            options.Password.RequireUppercase = false;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequiredLength = 6;

            // Lockout settings
            options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
            options.Lockout.MaxFailedAccessAttempts = 5;
            options.Lockout.AllowedForNewUsers = true;
        }

        public static void JwtBearerOptions(JwtBearerOptions options, IConfiguration configuration)
        {
            var jwtAppSettingsOptions = configuration.GetSection(nameof(JwtIssuerOptions));

            var symmetricSecurityKey =
                JwtSecurityKey.Create(jwtAppSettingsOptions[nameof(JwtIssuerOptions.IssuerSigningKey)]);
            var jwtIssuer = jwtAppSettingsOptions[nameof(JwtIssuerOptions.Issuer)];
            var jwtAudience = jwtAppSettingsOptions[nameof(JwtIssuerOptions.Audience)];

            // Jwt token validation configuration
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidIssuer = true.ToString(),
                ValidAudience = true.ToString(),
                ValidateLifetime = true,
                RequireExpirationTime = true,
                //ClockSkew = TimeSpan.Zero,

                ValidateIssuerSigningKey = true,
                ValidIssuers = new[] { jwtIssuer },
                ValidAudiences = new[] { jwtAudience },
                IssuerSigningKey = symmetricSecurityKey
            };

            // Jwt token configuration configuration
            options.Configuration = new OpenIdConnectConfiguration()
            {
                SigningKeys = { symmetricSecurityKey},
                Issuer = jwtIssuer
            };
            options.Audience = jwtAudience;
        }
    }
}