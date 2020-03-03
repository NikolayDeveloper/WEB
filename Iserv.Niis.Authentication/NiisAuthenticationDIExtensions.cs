using Microsoft.Extensions.DependencyInjection;

namespace Iserv.Niis.Authentication
{
    public static class NiisAuthenticationDIExtensions
    {
        public static IServiceCollection AddNiisAuthenticationServiceDependencies(this IServiceCollection serviceCollection)
        {
            serviceCollection
                .AddTransient<INiisUserAuthenticationService, NiisUserAuthenticationService>();

            return serviceCollection;
        }
    }
}
