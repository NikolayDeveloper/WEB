using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;

namespace Iserv.Niis.Portal.Infrastructure.StartupExtensions
{
    public static class CorsExtensions
    {
        public static IApplicationBuilder SetupCors(this IApplicationBuilder app, IConfigurationRoot configuration)
        {
            app.UseCors(builder => builder
                .AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod()
                .WithMethods("GET", "POST", "PUT", "DELETE")
                .WithExposedHeaders("Content-Disposition", "Origin","Accept", "Content-type", "x -total-count", "x-items-count", "x-current-page"));

            return app;
        }
    }
}
