using Iserv.Niis.Common.Codes;
using Iserv.Niis.DataAccess.EntityFramework;
using Iserv.Niis.Domain.Entities.Dictionaries;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;

namespace Iserv.Niis.Api.Infrastructure.InitialData
{
    public static partial class SeedConstantClaimsToDatabase
    {
        public static IApplicationBuilder DicPaymentStatusesSeedDataToDatabase(this IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<NiisWebContext>();

                var inidetData = new string[] { DicPaymentStatusCodes.Distributed, DicPaymentStatusCodes.NotDistributed, DicPaymentStatusCodes.Returned };

                var existsInidetData = context.Set<DicPaymentStatus>().Where(r => inidetData.Contains(r.Code)).ToList();

                if(existsInidetData.Any(r=>r.Code == DicPaymentStatusCodes.Distributed) == false)
                {
                    context.Set<DicPaymentStatus>().Add(new DicPaymentStatus
                    {
                        Code = DicPaymentStatusCodes.Distributed,
                        NameRu = "Распределённые",
                        NameKz = "Распределённые",
                        NameEn = "Distributed",
                        Description = "Сумма платежа полностью распределена"
                    });
                }

                if (existsInidetData.Any(r => r.Code == DicPaymentStatusCodes.NotDistributed) == false)
                {
                    context.Set<DicPaymentStatus>().Add(new DicPaymentStatus
                    {
                        Code = DicPaymentStatusCodes.NotDistributed,
                        NameRu = "Не распределённые",
                        NameKz = "Не распределённые",
                        NameEn = "NotDistributed",
                        Description = "Сумма платежа частично распределена, либо не распределена"
                    });
                }

                if (existsInidetData.Any(r => r.Code == DicPaymentStatusCodes.Returned) == false)
                {
                    context.Set<DicPaymentStatus>().Add(new DicPaymentStatus
                    {
                        Code = DicPaymentStatusCodes.Returned,
                        NameRu = "Возвращено",
                        NameKz = "Возвращено",
                        NameEn = "Returned",
                        Description = "Выполнен возврат платежа"
                    });
                }
                context.SaveChanges();
            }

            return app;
        }
    }
}
