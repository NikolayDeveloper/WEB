using Autofac;
using Iserv.Niis.ExternalServices.Features.Abstractions;
using Iserv.Niis.ExternalServices.Features.IntegrationContract.Feature;
using Iserv.Niis.ExternalServices.Features.IntegrationContract.Models;

namespace Iserv.Niis.ExternalServices.Host.Container.Modules
{
    public class PipelineModuleContract : Module
    {
        protected override void Load(ContainerBuilder builder)
        {

            builder.RegisterType<ContractApplicationSend.QueryPreLogging>()
                .As<AbstractPreLogging<ContractApplicationSend.Query>>();
            builder.RegisterType<ContractApplicationSend.QueryValidator>()
                .As<AbstractCommonValidate<ContractApplicationSend.Query>>();
            builder.RegisterType<ContractApplicationSend.QueryPostLogging>()
                .As<AbstractPostLogging<ContractApplicationSend.Query>>();
            builder.RegisterType<ContractApplicationSend.QueryException>()
                .As<AbstractionExceptionHandler<ContractApplicationSend.Query, ContractResponse>>();
        }
    }
}