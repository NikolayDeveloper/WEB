using AutoMapper;
using Iserv.Niis.Model.Mappings.Request;
using Microsoft.Extensions.DependencyInjection;

namespace Iserv.Niis.Portal.Infrastructure.StartupExtensions
{
    public static class AutoMapperExtensions
    {
        /// <summary>
        /// Регистрация сборок для AutoMapper
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddAutoMapperConfiguration(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(RequestDetailDtoProfile).Assembly);
            return services;
        }
    }
}
