using Iserv.Niis.Domain.Constants;
using Microsoft.Extensions.DependencyInjection;

namespace Iserv.Niis.Portal.Infrastructure.StartupExtensions
{
    public static class PolicyExtensions
    {
        /// <summary>
        ///     Конфигурация политик доступа к контроллерам и ресурсам
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddAuthorizationPolicies(this IServiceCollection services)
        {
            services.AddAuthorization(options =>
            {
                const string prmName = KeyFor.JwtClaimIdentifier.Permission;

                options.AddPolicy(KeyFor.Policy.HasAccessToJournal,
                    policy => policy.RequireClaim(prmName, KeyFor.Permission.JournalModule));

                options.AddPolicy(nameof(KeyFor.Policy.HasAccessToViewStaffTasks),
                    policy => policy.RequireClaim(prmName, KeyFor.Permission.JournalViewStaffTasks));

                options.AddPolicy(nameof(KeyFor.Policy.HasAccessToAdministration),
                    policy => policy.RequireClaim(prmName, KeyFor.Permission.AdministrationModule));

                //options.AddPolicy("resource-allow-policy",
                //    x => { x.AddRequirements(new ResourceBasedRequirement()); });
            });

            return services;
        }
    }
}