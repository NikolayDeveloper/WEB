using Autofac;
using Iserv.Niis.ExternalServices.Features.IntegrationIntelEGov.Utils;
using Iserv.Niis.ExternalServices.Features.IntegrationIntelEGov.Validations.Abstractions;
using Iserv.Niis.ExternalServices.Features.IntegrationIntelEGov.Validations.Implementations;

namespace Iserv.Niis.ExternalServices.Host.Container.Modules
{
    public class ValidatorsModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<IntegrationValidationHelper>();
            builder.RegisterType<ValidateCheckPatentStatementArgument>().As<IValidateCheckPatentStatementArgument>();
            builder.RegisterType<ValidateGetCountTextForPaySumArgument>().As<IValidateGetCountTextForPaySumArgument>();
            builder.RegisterType<ValidateGetPaySumArgument>().As<IValidateGetPaySumArgument>();
            builder.RegisterType<ValidateGetCustomerPatentValidityArgument>().As<IValidateGetCustomerPatentValidityArgument>();
            builder.RegisterType<ValidateMessageSendArgument>().As<IValidateMessageSendArgument>();
            builder.RegisterType<ValidateRequisitionSendArgument>().As<IValidateRequisitionSendArgument>();
        }
    }
}