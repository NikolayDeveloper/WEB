using Iserv.Niis.DI.DateTimeProviders;
using Microsoft.Extensions.DependencyInjection;

namespace Iserv.Niis.DI
{
    public static class NiisDIDependencyExtensions
    {
        public static IServiceCollection AddNiisDIDependencies(this IServiceCollection serviceCollection)
        {
            serviceCollection
                .AddTransient<IDateTimeProvider, DateTimeProvider>();

            return serviceCollection;
        }
    }
}
