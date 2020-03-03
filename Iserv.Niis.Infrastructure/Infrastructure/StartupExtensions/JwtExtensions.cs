using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;

namespace Iserv.Niis.Infrastructure.StartupExtensions
{
    public static class JwtExtensions
    {
        /// <summary>
        ///     Запрет доступа анонимным пользователям
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        public static MvcOptions DisableAnonimousAccess(this MvcOptions options)
        {
            var authorizePolicy = new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
                .Build();
            options.Filters.Add(new AuthorizeFilter(authorizePolicy));
            return options;
        }
    }
}