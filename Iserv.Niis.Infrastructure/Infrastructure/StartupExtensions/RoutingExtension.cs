using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace Iserv.Niis.Infrastructure.StartupExtensions
{
    public static class RoutingExtension
    {
        public static IApplicationBuilder SetupAngularRouting(this IApplicationBuilder app)
        {
            var env = (IHostingEnvironment)app.ApplicationServices.GetService(typeof(IHostingEnvironment));

            app.UseMvc(
                routes =>
                {
                    routes.MapRoute(
                        "api",
                        "api/{controller}/{action}/{id?}");
                });

            app.Run(context =>
            {
                if (!context.Request.Path.Value.StartsWith("/api"))
                {
                    context.Response.WriteAsync(env.WebRootPath != null
                        ? File.ReadAllText(Path.Combine(env.WebRootPath, "index.html"))
                        : string.Empty);
                }

                return Task.FromResult(0);
            });

            return app;
        }
    }
}
