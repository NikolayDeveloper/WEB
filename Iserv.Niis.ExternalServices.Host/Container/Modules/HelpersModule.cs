using Autofac;
using Iserv.Niis.ExternalServices.Features.IntegrationIntelEGov.Utils;
using Iserv.Niis.ExternalServices.Features.IntegrationSituationCenter.Utils;
using Iserv.Niis.ExternalServices.Features.Utils;
using Iserv.Niis.Infrastructure.Helpers;
using Iserv.Niis.Business.Helpers;

namespace Iserv.Niis.ExternalServices.Host.Container.Modules
{
    public class HelpersModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<IntegrationEnumMapper>();
            builder.RegisterType<IntegrationDictionaryHelper>();
            builder.RegisterType<IntegrationAttachFileHelper>();
            builder.RegisterType<IntegrationDocumentHelper>();
            builder.RegisterType<IntegrationEgovPayHelper>();
            builder.RegisterType<LoggingHelper>();
            builder.RegisterType<SystemInfoHelper>();
            builder.RegisterType<DictionaryHelper>();
            builder.RegisterType<SoapHelper>();
            builder.RegisterType<IntegrationEgovPayHelper>();
            builder.RegisterType<IntegrationRequisitionInfoHelper>();
            builder.RegisterType<AttachmentHelper>().As<IAttachmentHelper>();
            builder.RegisterType<ValidatePasswordHelper>();
        }
    }
}