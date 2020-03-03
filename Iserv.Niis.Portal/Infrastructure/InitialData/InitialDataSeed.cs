using Microsoft.AspNetCore.Builder;

namespace Iserv.Niis.Portal.Infrastructure.InitialData
{
    public static partial class InitialDataSeed
    {
        public static IApplicationBuilder SeedInitialData(this IApplicationBuilder app)
        {
            app.SeedConstantClaims().Wait();
            app.SeedDocumentTemplateFiles().Wait();

            return app;
        }
    }
}
