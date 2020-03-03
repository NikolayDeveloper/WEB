using Autofac;
using Iserv.Niis.Business.Abstract;
using Iserv.Niis.Business.Implementations;
using Iserv.Niis.Business.Notifications.Abstract;
using Iserv.Niis.Business.Notifications.Helpers;
using Iserv.Niis.Business.Notifications.Implementations;
using Iserv.Niis.Documents.Abstractions;
using Iserv.Niis.Documents.Infrastructure;
using Iserv.Niis.Documents.UserInput.Abstract;
using Iserv.Niis.Documents.UserInput.Implementations;
using Iserv.Niis.Domain.Entities.Contract;
using Iserv.Niis.Domain.Entities.Document;
using Iserv.Niis.Domain.Entities.ProtectionDoc;
using Iserv.Niis.Domain.Entities.Request;
using Iserv.Niis.Domain.Entities.Security;
using Iserv.Niis.Features.Helpers;
using Iserv.Niis.FileConverter.Abstract;
using Iserv.Niis.FileConverter.Implementations;
using Iserv.Niis.FileStorage.Abstract;
using Iserv.Niis.FileStorage.Implementations;
using Iserv.Niis.Infrastructure.Abstract;
using Iserv.Niis.Infrastructure.Implementations;
using Iserv.Niis.InternalServices.Features.IntegrationGbdFL.Abstractions;
using Iserv.Niis.InternalServices.Features.IntegrationGbdFL.Implementations;
using Iserv.Niis.InternalServices.Features.IntegrationGbdJur.Abstractions;
using Iserv.Niis.InternalServices.Features.IntegrationGbdJur.Implementations;
using Iserv.Niis.InternalServices.Features.Models;
using Iserv.Niis.Portal.Infrastructure.Security;
using Iserv.Niis.Workflow.Abstract;
using Iserv.Niis.Workflow.Implementations.Contract.Stages.PaymentApplier;
using Iserv.Niis.Workflow.Implementations.Document;
using Iserv.Niis.Workflow.Implementations.ProtectionDoc;
using Iserv.Niis.Workflow.Implementations.Request;
using Iserv.Niis.Workflow.Implementations.Request.Stages.DocumentApplier;
using Iserv.Niis.Workflow.Implementations.Request.Stages.GeneratedNumberApplier;
using Iserv.Niis.Workflow.Implementations.Request.Stages.SignedDocumentApplier;
using Iserv.Niis.Workflow.Implementations.Request.Stages.TransferedDocumentApplier;
using Iserv.Niis.Utils.Abstractions;
using Iserv.Niis.Utils.Helpers;
using Iserv.Niis.Utils.Implementations;
using Microsoft.AspNetCore.Identity;
using PaymentApplier = Iserv.Niis.Workflow.Implementations.Request.Stages.PaymentApplier.PaymentApplier;
using WorkflowApplier = Iserv.Niis.Workflow.Implementations.Contract.WorkflowApplier;

namespace Iserv.Niis.Portal.Mediator.AutofacContainer
{
    public class ServiceModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            return;

            builder.RegisterType<CustomerUpdater>()
                .As<ICustomerUpdater>();
            builder.RegisterType<ContractCategoryIdentifier>()
                .As<IContractCategoryIdentifier>();
            builder.RegisterType<Workflow.Implementations.Request.WorkflowApplier>()
                .As<IWorkflowApplier<Request>>();
            builder.RegisterType<Workflow.Implementations.Document.WorkflowApplier>()
                .As<IWorkflowApplier<Document>>();
            builder.RegisterType<Workflow.Implementations.ProtectionDoc.WorkflowApplier>()
                .As<IWorkflowApplier<ProtectionDoc>>();
            builder.RegisterType<DicTypeResolver>()
                .As<IDicTypeResolver>();
            builder.RegisterType<UserRolesUpdater>()
                .As<IUserRolesUpdater>();
            builder.RegisterType<UserIcgsUpdater>()
                .As<IUserIcgsUpdater>();
            builder.RegisterType<UserPasswordUpdater>()
                .As<IUserPasswordUpdater>();
            builder.RegisterType<RoleClaimsUpdater>()
                .As<IRoleClaimsUpdater>();
            builder.RegisterType<RoleRouteStagesUpdater>()
                .As<IRoleRouteStagesUpdater>();
            builder.RegisterType<LogoUpdater>()
                .As<ILogoUpdater>();
            builder.RegisterType<NumberGenerator>()
                .As<INumberGenerator>();
            builder.RegisterType<FileExporter>()
                .As<IFileExporter>();
	        
            builder.RegisterType<DocumentApplier>().As<IDocumentApplier<Request>>();
            builder.RegisterType<Workflow.Implementations.Request.Stages.DocumentApplier.LogicFactory>()
                .As<Workflow.Implementations.Request.Stages.DocumentApplier.ILogicFactory>();
            builder.RegisterType<Workflow.Implementations.Request.Stages.DocumentApplier.TrademarkLogic>();
            builder.RegisterType<Workflow.Implementations.Request.Stages.DocumentApplier.InventionLogic>();
            builder.RegisterType<Workflow.Implementations.Request.Stages.DocumentApplier.InternationalTrademarkLogic>();
	        builder.RegisterType<Workflow.Implementations.Request.Stages.DocumentApplier.UsefulModelLogic>();
	        builder.RegisterType<Workflow.Implementations.Request.Stages.DocumentApplier.IndustrialDesignLogic>();

            builder.RegisterType<Workflow.Implementations.Contract.Stages.DocumentApplier.DocumentApplier>().As<IDocumentApplier<Contract>>();
            builder.RegisterType<Workflow.Implementations.Contract.Stages.DocumentApplier.StageLogic>();

            builder.RegisterType<TransferedDocumentApplier>().As<ITransferedDocumentApplier>();
            builder.RegisterType<Workflow.Implementations.Request.Stages.TransferedDocumentApplier.LogicFactory>()
                .As<Workflow.Implementations.Request.Stages.TransferedDocumentApplier.ILogicFactory>();
            builder.RegisterType<Workflow.Implementations.Request.Stages.TransferedDocumentApplier.TrademarkLogic>();
            builder.RegisterType<Workflow.Implementations.Request.Stages.TransferedDocumentApplier.InventionLogic>();
            builder.RegisterType<Workflow.Implementations.Request.Stages.TransferedDocumentApplier.InternationalTrademarkLogic>();
	        builder.RegisterType<Workflow.Implementations.Request.Stages.TransferedDocumentApplier.UsefulModelLogic>();
	        builder.RegisterType<Workflow.Implementations.Request.Stages.TransferedDocumentApplier.IndustrialDesignLogic>();

			builder.RegisterType<SignedDocumentApplier>().As<ISignedDocumentApplier<Request>>();
            builder.RegisterType<Workflow.Implementations.Request.Stages.SignedDocumentApplier.LogicFactory>()
                .As<Workflow.Implementations.Request.Stages.SignedDocumentApplier.ILogicFactory>();
            builder.RegisterType<Workflow.Implementations.Request.Stages.SignedDocumentApplier.TrademarkLogic>();
            builder.RegisterType<Workflow.Implementations.Request.Stages.SignedDocumentApplier.InventionLogic>();
            builder.RegisterType<Workflow.Implementations.Request.Stages.SignedDocumentApplier.InternationalTrademarkLogic>();

            builder.RegisterType<Workflow.Implementations.Contract.Stages.SignedDocumentApplier.SignedDocumentApplier>().As<ISignedDocumentApplier<Contract>>();
            builder.RegisterType<Workflow.Implementations.Contract.Stages.SignedDocumentApplier.StageLogic>();

            builder.RegisterType<PaymentApplier>().As<IPaymentApplier<Request>>();
            builder.RegisterType<Workflow.Implementations.Request.Stages.PaymentApplier.LogicFactory>()
                .As<Workflow.Implementations.Request.Stages.PaymentApplier.ILogicFactory>();
            builder.RegisterType<Workflow.Implementations.Request.Stages.PaymentApplier.TrademarkLogic>();
            builder.RegisterType<Workflow.Implementations.Request.Stages.PaymentApplier.InventionLogic>();
            builder.RegisterType<Workflow.Implementations.Request.Stages.PaymentApplier.InternationalTrademarkLogic>();
	        builder.RegisterType<Workflow.Implementations.Request.Stages.PaymentApplier.UsefulModelLogic>();
	        builder.RegisterType<Workflow.Implementations.Request.Stages.PaymentApplier.IndustrialDesignLogic>();

            builder.RegisterType<GeneratedNumberApplier>().As<IGeneratedNumberApplier<Request>>();
            builder.RegisterType<Workflow.Implementations.Request.Stages.GeneratedNumberApplier.LogicFactory>()
                .As<Workflow.Implementations.Request.Stages.GeneratedNumberApplier.ILogicFactory>();
            builder.RegisterType<Workflow.Implementations.Request.Stages.GeneratedNumberApplier.InternationalTrademarkLogic>();
            builder.RegisterType<Workflow.Implementations.Request.Stages.GeneratedNumberApplier.UsefulModelLogic>();
            builder.RegisterType<Workflow.Implementations.Request.Stages.GeneratedNumberApplier.IndustrialDesignLogic>();

            builder.RegisterType<Workflow.Implementations.Contract.Stages.PaymentApplier.PaymentApplier>().As<IPaymentApplier<Contract>>();
            builder.RegisterType<StageLogic>();
            builder.RegisterType<Workflow.Implementations.Contract.Stages.GeneratedNumberApplier.GeneratedNumberApplier>().As<IGeneratedNumberApplier<Contract>>();
            builder.RegisterType<Workflow.Implementations.Contract.Stages.GeneratedNumberApplier.StageLogic>();

            builder.RegisterType<Workflow.Implementations.Request.TaskRegister>().As<ITaskRegister<Request>>();
            builder.RegisterType<Workflow.Implementations.Contract.TaskRegister>().As<ITaskRegister<Contract>>();
            builder.RegisterType<NotificationSender>().As<INotificationSender>();

            builder.RegisterType<SendHelper>()
                .As<ISendHelper>();

            builder.RegisterType<AppClaimsPrincipalFactory>()
                .As<IUserClaimsPrincipalFactory<ApplicationUser>>()
                .InstancePerLifetimeScope();
            builder.RegisterType<JwtFactory>()
                .As<IJwtFactory>();

            builder.RegisterType<CertificateService>()
                .As<ICertificateService>();

            builder.RegisterAssemblyTypes(typeof(DocumentGeneratorFactory).Assembly);
            builder.RegisterType<DocumentGeneratorFactory>()
                .As<IDocumentGeneratorFactory>();
            builder.RegisterType<TemplateFieldValueFactory>()
                .As<ITemplateFieldValueFactory>();
            builder.RegisterType<TemplateUserInputChecker>()
                .As<ITemplateUserInputChecker>();
            builder.RegisterType<AsposeFileConverter>()
                .As<IFileConverter>();
            builder.RegisterType<AttachmentHelper>()
                .As<IAttachmentHelper>();
            builder.RegisterType<MinioFileStorage>()
                .As<IFileStorage>();
            builder.RegisterType<DocxTemplateHelper>()
                .As<IDocxTemplateHelper>();

            builder.RegisterType<CalendarProvider>()
                .As<ICalendarProvider>();
            builder.RegisterType<DocumentsCompare>()
                .As<IDocumentsCompare>();

            builder.RegisterType<WorkflowApplier>()
                .As<IWorkflowApplier<Contract>>();
            builder.RegisterType<ContractRequestRelationUpdater>()
                .As<IContractRequestRelationUpdater>();

            builder.RegisterType<NotificationsFactory>()
                .As<INotificationsFactory>();
            builder.RegisterType<GenerateHash>().As<IGenerateHash>();
            builder.RegisterType<IntegrationDocumentUpdater>().As<IIntegrationDocumentUpdater>();
            builder.RegisterType<IntegrationStatusUpdater>().As<IIntegrationStatusUpdater>();
            builder.RegisterType<DictionaryHelper>();
            builder.RegisterType<GbdJuridicalService>()
                .As<IGbdJuridicalService>();
            builder.RegisterType<GbdFlService>()
                .As<IGbdFlService>();
            builder.RegisterType<ConfigExternalService>();
            builder.RegisterType<NotificationTaskResolver>()
                .As<INotificationTaskResolver>();
            builder.RegisterType<NotificationTaskRegister>()
                .As<INotificationTaskRegister>();
        }
    }
}