using Microsoft.AspNetCore.Builder;

namespace Iserv.Niis.Api.Infrastructure.InitialData
{
    public static partial class SeedConstantClaimsToDatabase
    {
        public static IApplicationBuilder SeedInitialData(this IApplicationBuilder app)
        {
            //app.SeedConstantClaims().Wait();
            app.SeedDocumentTemplateFiles();
            //app.DicPaymentStatusesSeedDataToDatabase();



            return app;
        }
    }
}
