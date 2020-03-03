using Autofac;
using Iserv.Niis.DataAccess.EntityFramework;
using Iserv.Niis.ExternalServices.DataAccess.EntityFramework;
using Microsoft.EntityFrameworkCore;
using System.Configuration;

namespace Iserv.Niis.ExternalServices.Host.Container.Modules
{
    public class DataContextsModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            var niisIntegrationOptionsBuilder =
                new DbContextOptionsBuilder<NiisIntegrationContext>()
                    .UseSqlServer(ConfigurationManager.AppSettings[Constants.AppSettings.EgovConfiguration.ConStringNiisIntegration]);

            // todo: Reduce command timeout to smaller value
            var niisOptionsBuilder = new DbContextOptionsBuilder<NiisWebContext>()
                    .UseSqlServer(ConfigurationManager.AppSettings[Constants.AppSettings.EgovConfiguration.ConStringNiisWeb],
                        sqlServerOptions => sqlServerOptions.CommandTimeout(10000));

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