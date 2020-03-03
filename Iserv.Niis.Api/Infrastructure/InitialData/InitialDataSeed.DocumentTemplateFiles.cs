using Iserv.Niis.Business.Helpers;
using Iserv.Niis.DataAccess.EntityFramework;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace Iserv.Niis.Api.Infrastructure.InitialData
{
    public static partial class SeedConstantClaimsToDatabase
    {
        private static void SeedDocumentTemplateFiles(this IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<NiisWebContext>();
                var hostingEnvironment = app.ApplicationServices.GetService<IHostingEnvironment>();
                var path = System.Environment.CurrentDirectory;
                DocumentTemplateHelper.UpdateTemplateEntities(context, path);
            }
        }
    }
}