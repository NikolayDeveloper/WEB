using Autofac;
using Iserv.Niis.Business.Abstract;
using Iserv.Niis.Business.Implementations;
using Iserv.Niis.ExternalServices.Features.IntegrationContract.Abstractions;
using Iserv.Niis.ExternalServices.Features.IntegrationContract.Feature;
using Iserv.Niis.ExternalServices.Features.IntegrationContract.Implementations;
using Iserv.Niis.ExternalServices.Features.IntegrationIntelEGov.Abstractions;
using Iserv.Niis.ExternalServices.Features.IntegrationIntelEGov.Implementations;
using Iserv.Niis.ExternalServices.Features.IntegrationSituationCenter.Abstractions;
using Iserv.Niis.ExternalServices.Features.IntegrationSituationCenter.Implementations;
using Iserv.Niis.FileStorage.Abstract;
using Iserv.Niis.FileStorage.Implementations;
using Iserv.Niis.Utils.Abstractions;
using Iserv.Niis.Utils.Implementations;

namespace Iserv.Niis.ExternalServices.Host.Container.Modules
{
    public class ServicesModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<MessageSendService>().As<IMessageSendService>();
            builder.RegisterType<RequisitionSendService>().As<IRequisitionSendService>();
            builder.RegisterType<GetAttachedFileMetadataService>().As<IGetAttachedFileMetadataService>();
            builder.RegisterType<MinioFileStorage>().As<IFileStorage>();
            builder.RegisterType<NumberGenerator>().As<INumberGenerator>();
            builder.RegisterType<GetPaySumService>().As<IGetPaySumService>();
            builder.RegisterType<GetCustomerPatentValidityService>().As<IGetCustomerPatentValidityService>();
            builder.RegisterType<ContractApplicationSendService>().As<IContractApplicationSendService>();
            builder.RegisterType<GetAttorneyInfoService>().As<IGetAttorneyInfoService>();
            builder.RegisterType<GenerateHash>().As<IGenerateHash>();
            builder.RegisterType<GetRequisitionListByMessageTypeService>().As<IGetRequisitionListByMessageTypeService>();
            builder.RegisterType<GetRequisitionListForPaymentService>().As<IGetRequisitionListForPaymentService>();
            builder.RegisterType<GetRequisitionInfoService>().As<IGetRequisitionInfoService>();
            builder.RegisterType<GetMessageFileService>().As<IGetMessageFileService>();
            builder.RegisterType<CheckPatentStatementService>().As<ICheckPatentStatementService>();
            builder.RegisterType<GetCountTextForPaySumService>().As<IGetCountTextForPaySumService>();
            builder.RegisterType<GetPayTariffService>().As<IGetPayTariffService>();
            builder.RegisterType<DocumentService>().As<IDocumentService>();
            builder.RegisterType<ProtectionDocService>().As<IProtectionDocService>();
            builder.RegisterType<RfProtectionDocService>().As<IRfProtectionDocService>();
            builder.RegisterType<TypeInfoService>().As<ITypeInfoService>();
            builder.RegisterType<IntegrationStatusUpdater>().As<IIntegrationStatusUpdater>();
            builder.RegisterType<DicTypeResolver>().As<IDicTypeResolver>();
        }
    }
}