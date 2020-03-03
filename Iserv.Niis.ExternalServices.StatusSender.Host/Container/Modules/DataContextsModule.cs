using System.Configuration;
using Autofac;
using Iserv.Niis.DataAccess.EntityFramework;
using Iserv.Niis.ExternalServices.DataAccess.EntityFramework;
using Iserv.Niis.ExternalServices.StatusSender.Host.Constants;
using Microsoft.EntityFrameworkCore;

namespace Iserv.Niis.ExternalServices.StatusSender.Host.Container.Modules
{
    public class DataContextsModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            var niisIntegrationOptionsBuilder =
                new DbContextOptionsBuilder<NiisIntegrationContext>()
                    .UseSqlServer(ConfigurationManager.AppSettings[AppSettings.ConStringNiisIntegration]);

            var niisOptionsBuilder =
                new DbContextOptionsBuilder<NiisWebContext>()
                    .UseSqlServer(ConfigurationManager.AppSettings[AppSettings.ConStringNiisWeb]);

            builder.RegisterInstance(niisOptionsBuilder.Options)
                .As<DbContextOptions<NiisWebContext>>()
                .SingleInstance();

            builder.RegisterInstance(niisIntegrationOptionsBuilder.Options)
                .As<DbContextOptions<NiisIntegrationContext>>()
                .SingleInstance();

            builder.RegisterType<NiisIntegrationContext>()
                .SingleInstance();

            builder.RegisterType<NiisWebContext>()
                .SingleInstance();
        }
    }
}
