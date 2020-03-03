using System;
using Autofac;
using Iserv.Niis.ExternalServices.Host.Container.Modules;
using Iserv.Niis.ExternalServices.Host.Utils;
using Microsoft.Extensions.Configuration;

namespace Iserv.Niis.ExternalServices.Host.Container
{
    public class AutofacContainer
    {
        public IContainer Build()
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json")
                .Build();
            SerilogConfig.Configuration();

            var builder = new ContainerBuilder();
            builder.RegisterInstance(configuration).As<IConfiguration>();
            builder.RegisterModule<AutoMapperModule>();
            builder.RegisterModule<DataContextsModule>();
            builder.RegisterModule<HelpersModule>();
            builder.RegisterModule<MediatorModule>();
            builder.RegisterModule<OptionsModule>();
            builder.RegisterModule<ValidatorsModule>();
            builder.RegisterModule<PipelineModuleIEgov>();
            builder.RegisterModule<PipelineModuleContract>();
            builder.RegisterModule<PipelineModuleSituationCenter>();
            builder.RegisterModule<ServicesModule>();

            return builder.Build();
        }
    }
}