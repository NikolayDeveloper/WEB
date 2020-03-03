using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Iserv.Niis.DataAccess.EntityFramework;
using Iserv.Niis.Domain.Constants;
using Iserv.Niis.Domain.Entities.Security;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using RestSharp.Extensions;

namespace Iserv.Niis.Api.Infrastructure.InitialData
{
    public static partial class SeedConstantClaimsToDatabase
    {
        /// <summary>
        /// Добавляет в БД список клэймов с описанием.
        /// Требуется для отображения списка при создании роли поьзователя
        /// Добавлена исключительно для удобства 
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        private static async Task<IApplicationBuilder> SeedConstantClaims(this IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<NiisWebContext>();

                var existingValues = await GetExistingConstantValuesAsync(context);

                var claimsToInsert = GetClaimsToInsert(existingValues);

                await InsertNewClaimConstantsAsync(context, claimsToInsert);

                return app;
            }

        }

        private static async Task<List<string>> GetExistingConstantValuesAsync(NiisWebContext context)
        {
            return await context.ClaimConstants
                .Select(cc => cc.Value)
                .ToListAsync();
        }

        private static List<ClaimConstant> GetClaimsToInsert(List<string> existingValues)
        {
            return GetConstantDictionary()
                .Where(c => !existingValues.Any(cc => cc.Equals(c.Value)))
                .ToList();
        }

        private static async Task InsertNewClaimConstantsAsync(NiisWebContext context, List<ClaimConstant> claimsToInsert)
        {
            context.ClaimConstants.AddRange(claimsToInsert);

            await context.SaveChangesAsync();
        }

        private static IEnumerable<ClaimConstant> GetConstantDictionary()
        {
            return typeof(KeyFor.Permission)
                .GetFields()
                .Select(x =>
                new ClaimConstant
                {
                    FieldName = x.Name,
                    Value = (string)x.GetValue(null),
                    NameRu = x.GetAttribute<DisplayAttribute>().Name,
                    NameKz = x.GetAttribute<DisplayAttribute>().Name,
                    NameEn = x.GetAttribute<DisplayAttribute>().Name
                }).ToList();
        }
    }
}
