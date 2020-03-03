using Autofac;
using Iserv.Niis.ExternalServices.Features.IntegrationIntelStatusSender.Abstractions;
using Iserv.Niis.ExternalServices.Features.IntegrationIntelStatusSender.Helpers;
using Iserv.Niis.ExternalServices.Features.IntegrationIntelStatusSender.Implementations;
using Iserv.Niis.ExternalServices.Features.Utils;
using Iserv.Niis.ExternalServices.StatusSender.Host.Container.Modules;

namespace Iserv.Niis.ExternalServices.StatusSender.Host.Container
{
    public static class AutofacContainerBuilder
    {
        public static IContainer BuildContainer()
        {
            var builder = new ContainerBuilder();
            builder.RegisterModule<OptionsModule>();
            builder.RegisterModule<DataContextsModule>();
            builder.RegisterModule<AutoMapperModule>();

            builder.RegisterType<KazPatentCabinetHelper>();
            builder.RegisterType<PepHelper>();
            builder.RegisterType<LoggingHelper>();

            builder.RegisterType<WinServiceStatusSender>().As<IWinServiceStatusSender>();
            builder.RegisterType<IntegrationDocumentService>().As<IIntegrationDocumentService>();
            builder.RegisterType<IntegrationRequisitionService>().As<IIntegrationRequisitionService>();
            builder.RegisterType<IntegrationStatusService>().As<IIntegrationStatusService>();
            builder.RegisterType<Session>().As<ISession>();
            return builder.Build();
        }
    }
}