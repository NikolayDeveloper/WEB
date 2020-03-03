using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace Iserv.Niis.Portal.Infrastructure.StartupExtensions
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
                        name: "api",
                        template: "api/{controller}/{action}/{id?}");
                });

            app.Run(context =>
            {
                if (!context.Request.Path.Value.StartsWith("/api"))
                {
                    if (env.WebRootPath != null)
                    {
                        context.Response.WriteAsync(File.ReadAllText(Path.Combine(env.WebRootPath, "index.html")));
                    }
                    else
                    {
                        context.Response.WriteAsync(string.Empty);
                    }
                }

                //if (Path.HasExtension(context.Request.Path.Value))
                //{
                //    var indexFilePath = Path.Combine(env.WebRootPath, context.Request.Path.Value);
                //    if (File.Exists(indexFilePath))
                //    {
                //        context.Response.WriteAsync(File.ReadAllText(indexFilePath));
                //    }
                //}
                return Task.FromResult(0);
            });

            return app;
        }
    }
}
