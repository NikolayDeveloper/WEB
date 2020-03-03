using Autofac;
using Iserv.Niis.ExternalServices.Features.Abstractions;
using Iserv.Niis.ExternalServices.Features.IntegrationIntelEGov.Feature;
using Iserv.Niis.ExternalServices.Features.IntegrationIntelEGov.Models;
using System.Collections.Generic;
using Iserv.Niis.ExternalServices.Features.IntegrationContract.Models;

namespace Iserv.Niis.ExternalServices.Host.Container.Modules
{
    public class PipelineModuleIEgov : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<RequisitionSend.CommandPreLogging>()
                .As<AbstractPreLogging<RequisitionSend.Command>>();
            builder.RegisterType<RequisitionSend.CommandValidator>()
                .As<AbstractCommonValidate<RequisitionSend.Command>>();
            builder.RegisterType<RequisitionSend.CommandPostLogging>()
                .As<AbstractPostLogging<RequisitionSend.Command>>();
            builder.RegisterType<RequisitionSend.CommandException>()
                .As<AbstractionExceptionHandler<RequisitionSend.Command, RequisitionSendResult>>();

            builder.RegisterType<GetAttorneyInfo.QueryPreLogging>()
                .As<AbstractPreLogging<GetAttorneyInfo.Query>>();
            builder.RegisterType<GetAttorneyInfo.QueryValidator>()
                .As<AbstractCommonValidate<GetAttorneyInfo.Query>>();
            builder.RegisterType<GetAttorneyInfo.QueryPostLogging>()
                .As<AbstractPostLogging<GetAttorneyInfo.Query>>();
            builder.RegisterType<GetAttorneyInfo.QueryException>()
                .As<AbstractionExceptionHandler<GetAttorneyInfo.Query, GetAttorneyInfoResult>>();

            builder.RegisterType<MessageSend.CommandPreLogging>()
                .As<AbstractPreLogging<MessageSend.Command>>();
            builder.RegisterType<MessageSend.CommandValidator>()
                .As<AbstractCommonValidate<MessageSend.Command>>();
            builder.RegisterType<MessageSend.CommandPostLogging>()
                .As<AbstractPostLogging<MessageSend.Command>>();
            builder.RegisterType<MessageSend.CommandException>()
                .As<AbstractionExceptionHandler<MessageSend.Command, MessageSendResult>>();

            builder.RegisterType<GetRequisitionListByMessageType.QueryPreLogging>()
                .As<AbstractPreLogging<GetRequisitionListByMessageType.Query>>();
            builder.RegisterType<GetRequisitionListByMessageType.QueryValidator>()
                .As<AbstractCommonValidate<GetRequisitionListByMessageType.Query>>();
            builder.RegisterType<GetRequisitionListByMessageType.QueryPostLogging>()
                .As<AbstractPostLogging<GetRequisitionListByMessageType.Query>>();
            builder.RegisterType<GetRequisitionListByMessageType.QueryException>()
                .As<AbstractionExceptionHandler<GetRequisitionListByMessageType.Query,
                    GetRequisitionListByMessageTypeResult>>();

            builder.RegisterType<CheckPatentStatement.QueryPreLogging>()
                .As<AbstractPreLogging<CheckPatentStatement.Query>>();
            builder.RegisterType<CheckPatentStatement.QueryValidator>()
                .As<AbstractCommonValidate<CheckPatentStatement.Query>>();
            builder.RegisterType<CheckPatentStatement.QueryPostLogging>()
                .As<AbstractPostLogging<CheckPatentStatement.Query>>();
            builder.RegisterType<CheckPatentStatement.QueryException>()
                .As<AbstractionExceptionHandler<CheckPatentStatement.Query, CheckPatentStatementResult>>();

            builder.RegisterType<GetAttachedFileMetadata.QueryPreLogging>()
                .As<AbstractPreLogging<GetAttachedFileMetadata.Query>>();
            builder.RegisterType<GetAttachedFileMetadata.QueryValidator>()
                .As<AbstractCommonValidate<GetAttachedFileMetadata.Query>>();
            builder.RegisterType<GetAttachedFileMetadata.QueryPostLogging>()
                .As<AbstractPostLogging<GetAttachedFileMetadata.Query>>();
            builder.RegisterType<GetAttachedFileMetadata.QueryException>()
                .As<AbstractionExceptionHandler<GetAttachedFileMetadata.Query, GetAttachedFileMetadataResult>>();

            builder.RegisterType<GetCountTextForPaySum.QueryPreLogging>()
                .As<AbstractPreLogging<GetCountTextForPaySum.Query>>();
            builder.RegisterType<GetCountTextForPaySum.QueryValidator>()
                .As<AbstractCommonValidate<GetCountTextForPaySum.Query>>();
            builder.RegisterType<GetCountTextForPaySum.QueryPostLogging>()
                .As<AbstractPostLogging<GetCountTextForPaySum.Query>>();
            builder.RegisterType<GetCountTextForPaySum.QueryException>()
                .As<AbstractionExceptionHandler<GetCountTextForPaySum.Query, GetCountTextForPaySumResult>>();

            builder.RegisterType<GetMessageFile.QueryPreLogging>()
                .As<AbstractPreLogging<GetMessageFile.Query>>();
            builder.RegisterType<GetMessageFile.QueryValidator>()
                .As<AbstractCommonValidate<GetMessageFile.Query>>();
            builder.RegisterType<GetMessageFile.QueryPostLogging>()
                .As<AbstractPostLogging<GetMessageFile.Query>>();
            builder.RegisterType<GetMessageFile.QueryException>()
                .As<AbstractionExceptionHandler<GetMessageFile.Query, GetMessageFileResult>>();

            builder.RegisterType<GetPaySum.QueryPreLogging>()
                .As<AbstractPreLogging<GetPaySum.Query>>();
            builder.RegisterType<GetPaySum.QueryValidator>()
                .As<AbstractCommonValidate<GetPaySum.Query>>();
            builder.RegisterType<GetPaySum.QueryPostLogging>()
                .As<AbstractPostLogging<GetPaySum.Query>>();
            builder.RegisterType<GetPaySum.QueryException>()
                .As<AbstractionExceptionHandler<GetPaySum.Query, GetPaySumResult>>();

            builder.RegisterType<GetCustomerPatentValidity.QueryPreLogging>()
                .As<AbstractPreLogging<GetCustomerPatentValidity.Query>>();
            builder.RegisterType<GetCustomerPatentValidity.QueryValidator>()
                .As<AbstractCommonValidate<GetCustomerPatentValidity.Query>>();
            builder.RegisterType<GetCustomerPatentValidity.QueryPostLogging>()
                .As<AbstractPostLogging<GetCustomerPatentValidity.Query>>();
            builder.RegisterType<GetCustomerPatentValidity.QueryException>()
                .As<AbstractionExceptionHandler<GetCustomerPatentValidity.Query, CustomerPatentValidityResponce>>();

            builder.RegisterType<GetPayTariff.QueryPreLogging>()
                .As<AbstractPreLogging<GetPayTariff.Query>>();
            builder.RegisterType<GetPayTariff.QueryValidator>()
                .As<AbstractCommonValidate<GetPayTariff.Query>>();
            builder.RegisterType<GetPayTariff.QueryPostLogging>()
                .As<AbstractPostLogging<GetPayTariff.Query>>();
            builder.RegisterType<GetPayTariff.QueryException>()
                .As<AbstractionExceptionHandler<GetPayTariff.Query, GetPayTarifResult>>();

            builder.RegisterType<GetRequisitionInfo.QueryPreLogging>()
                .As<AbstractPreLogging<GetRequisitionInfo.Query>>();
            builder.RegisterType<GetRequisitionInfo.QueryValidator>()
                .As<AbstractCommonValidate<GetRequisitionInfo.Query>>();
            builder.RegisterType<GetRequisitionInfo.QueryPostLogging>()
                .As<AbstractPostLogging<GetRequisitionInfo.Query>>();
            builder.RegisterType<GetRequisitionInfo.QueryException>()
                .As<AbstractionExceptionHandler<GetRequisitionInfo.Query, GetRequisitionInfoResult>>();

            builder.RegisterType<GetRequisitionListForPayment.QueryPreLogging>()
                .As<AbstractPreLogging<GetRequisitionListForPayment.Query>>();
            builder.RegisterType<GetRequisitionListForPayment.QueryValidator>()
                .As<AbstractCommonValidate<GetRequisitionListForPayment.Query>>();
            builder.RegisterType<GetRequisitionListForPayment.QueryPostLogging>()
                .As<AbstractPostLogging<GetRequisitionListForPayment.Query>>();
            builder.RegisterType<GetRequisitionListForPayment.QueryException>()
                .As<AbstractionExceptionHandler<GetRequisitionListForPayment.Query, GetRequisitionListForPaymentResult>>();

            builder.RegisterType<GetAllTariffs.QueryPreLogging>()
                .As<AbstractPreLogging<GetAllTariffs.Query>>();
            builder.RegisterType<GetAllTariffs.QueryValidator>()
                .As<AbstractCommonValidate<GetAllTariffs.Query>>();
            builder.RegisterType<GetAllTariffs.QueryPostLogging>()
                .As<AbstractPostLogging<GetAllTariffs.Query>>();
            builder.RegisterType<GetAllTariffs.QueryException>()
                .As<AbstractionExceptionHandler<GetAllTariffs.Query, List<Tariff>>>();

            builder.RegisterType<GetAllColors.QueryPreLogging>()
                .As<AbstractPreLogging<GetAllColors.Query>>();
            builder.RegisterType<GetAllColors.QueryValidator>()
                .As<AbstractCommonValidate<GetAllColors.Query>>();
            builder.RegisterType<GetAllColors.QueryPostLogging>()
                .As<AbstractPostLogging<GetAllColors.Query>>();
            builder.RegisterType<GetAllColors.QueryException>()
                .As<AbstractionExceptionHandler<GetAllColors.Query, List<Color>>>();

            builder.RegisterType<GetAllDocuments.QueryPreLogging>()
                .As<AbstractPreLogging<GetAllDocuments.Query>>();
            builder.RegisterType<GetAllDocuments.QueryValidator>()
                .As<AbstractCommonValidate<GetAllDocuments.Query>>();
            builder.RegisterType<GetAllDocuments.QueryPostLogging>()
                .As<AbstractPostLogging<GetAllDocuments.Query>>();
            builder.RegisterType<GetAllDocuments.QueryException>()
                .As<AbstractionExceptionHandler<GetAllDocuments.Query, List<DocumentInfo>>>();

            builder.RegisterType<GetAllPatentAttorneys.QueryPreLogging>()
                .As<AbstractPreLogging<GetAllPatentAttorneys.Query>>();
            builder.RegisterType<GetAllPatentAttorneys.QueryValidator>()
                .As<AbstractCommonValidate<GetAllPatentAttorneys.Query>>();
            builder.RegisterType<GetAllPatentAttorneys.QueryPostLogging>()
                .As<AbstractPostLogging<GetAllPatentAttorneys.Query>>();
            builder.RegisterType<GetAllPatentAttorneys.QueryException>()
                .As<AbstractionExceptionHandler<GetAllPatentAttorneys.Query, List<PatentAttorney>>>();


            builder.RegisterType<GetCustomerPatentInfo.QueryPreLogging>()
                .As<AbstractPreLogging<GetCustomerPatentInfo.Query>>();
            builder.RegisterType<GetCustomerPatentInfo.QueryValidator>()
                .As<AbstractCommonValidate<GetCustomerPatentInfo.Query>>();
            builder.RegisterType<GetCustomerPatentInfo.QueryPostLogging>()
                .As<AbstractPostLogging<GetCustomerPatentInfo.Query>>();
            builder.RegisterType<GetCustomerPatentInfo.QueryException>()
                .As<AbstractionExceptionHandler<GetCustomerPatentInfo.Query, List<CustomerPatentInfo>>>();
        }
    }
}