using Autofac;
using Iserv.Niis.ExternalServices.Features.Abstractions;
using Iserv.Niis.ExternalServices.Features.IntegrationSituationCenter.Features;
using Iserv.Niis.ExternalServices.Features.IntegrationSituationCenter.Models;

namespace Iserv.Niis.ExternalServices.Host.Container.Modules
{
    public class PipelineModuleSituationCenter : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<DocumentList.QueryPreLogging>()
                .As<AbstractPreLogging<DocumentList.Query>>();
            builder.RegisterType<DocumentList.QueryValidator>()
                .As<AbstractCommonValidate<DocumentList.Query>>();
            builder.RegisterType<DocumentList.QueryPostLogging>()
                .As<AbstractPostLogging<DocumentList.Query>>();
            builder.RegisterType<DocumentList.QueryException>()
                .As<AbstractionExceptionHandler<DocumentList.Query, GetDocumentListResult>>();

            builder.RegisterType<ProtectionDocList.QueryPreLogging>()
                .As<AbstractPreLogging<ProtectionDocList.Query>>();
            builder.RegisterType<ProtectionDocList.QueryValidator>()
                .As<AbstractCommonValidate<ProtectionDocList.Query>>();
            builder.RegisterType<ProtectionDocList.QueryPostLogging>()
                .As<AbstractPostLogging<ProtectionDocList.Query>>();
            builder.RegisterType<ProtectionDocList.QueryException>()
                .As<AbstractionExceptionHandler<ProtectionDocList.Query, GetBtBasePatentListResult>>();

            builder.RegisterType<RfProtectionDocList.QueryPreLogging>()
                .As<AbstractPreLogging<RfProtectionDocList.Query>>();
            builder.RegisterType<RfProtectionDocList.QueryValidator>()
                .As<AbstractCommonValidate<RfProtectionDocList.Query>>();
            builder.RegisterType<RfProtectionDocList.QueryPostLogging>()
                .As<AbstractPostLogging<RfProtectionDocList.Query>>();
            builder.RegisterType<RfProtectionDocList.QueryException>()
                .As<AbstractionExceptionHandler<RfProtectionDocList.Query, GetRfPatentListResult>>();

            builder.RegisterType<TypeInfoList.QueryPreLogging>()
                .As<AbstractPreLogging<TypeInfoList.Query>>();
            builder.RegisterType<TypeInfoList.QueryValidator>()
                .As<AbstractCommonValidate<TypeInfoList.Query>>();
            builder.RegisterType<TypeInfoList.QueryPostLogging>()
                .As<AbstractPostLogging<TypeInfoList.Query>>();
            builder.RegisterType<TypeInfoList.QueryException>()
                .As<AbstractionExceptionHandler<TypeInfoList.Query, GetReferenceResult>>();
        }
    }
}