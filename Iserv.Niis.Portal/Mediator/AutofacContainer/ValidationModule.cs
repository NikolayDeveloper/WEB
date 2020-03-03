using Autofac;
using FluentValidation;
using System.Reflection;
using Iserv.Niis.Features.Contract;
using Create = Iserv.Niis.Features.Contract.Create;
using Delete = Iserv.Niis.Features.Contract.Delete;
using Module = Autofac.Module;

namespace Iserv.Niis.Portal.Mediator.AutofacContainer
{
    public class ValidationModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            var assembly = typeof(Features.Request.Create.CommandValidator).GetTypeInfo().Assembly;

            builder.RegisterAssemblyTypes(assembly)
                .Where(t => t.IsClosedTypeOf(typeof(AbstractValidator<>)))
                .AsSelf()
                .InstancePerLifetimeScope();

            //Заявки
            builder.RegisterType<Features.Request.Create.CommandValidator>()
                .As<AbstractValidator<Features.Request.Create.Command>>();
            builder.RegisterType<Features.Request.Single.QueryValidator>()
                .As<AbstractValidator<Features.Request.Single.Query>>();
            builder.RegisterType<Features.Request.ListByCode.QueryValidator>()
                .As<AbstractValidator<Features.Request.ListByCode.Query>>();
            builder.RegisterType<Features.Request.Update.CommandValidator>()
                .As<AbstractValidator<Features.Request.Update.Command>>();
            builder.RegisterType<Features.Request.Delete.CommandValidator>()
                .As<AbstractValidator<Features.Request.Delete.Command>>();
            builder.RegisterType<Features.Request.UploadImage.CommandValidator>()
                .As<AbstractValidator<Features.Request.UploadImage.Command>>();
            builder.RegisterType<Features.Request.SingleImage.CommandValidator>()
                .As<AbstractValidator<Features.Request.SingleImage.Command>>();
            builder.RegisterType<Features.Request.ShortInformationList.QueryValidator>()
                .As<AbstractValidator<Features.Request.ShortInformationList.Query>>();
		    builder.RegisterType<Features.Request.GenerateRequestNumber.CommandValidator>()
		        .As<AbstractValidator<Features.Request.GenerateRequestNumber.Command>>();

            builder.RegisterType<Features.Workflow.Create.CommandValidator>()
                .As<AbstractValidator<Features.Workflow.Create.Command>>();
            builder.RegisterType<Features.Workflow.List.QueryValidator>()
                .As<AbstractValidator<Features.Workflow.List.Query>>();
            builder.RegisterType<Features.Workflow.GetStageUserOptions.QueryValidator>()
                .As<AbstractValidator<Features.Workflow.GetStageUserOptions.Query>>();

            builder.RegisterType<Features.Request.ICGSRequestListByRequestIds.QueryValidator>()
                .As<AbstractValidator<Features.Request.ICGSRequestListByRequestIds.Query>>();

            //Опции выбора
            builder.RegisterType<Features.Dictionary.SelectOption.List.QueryValidator>()
                .As<AbstractValidator<Features.Dictionary.SelectOption.List.Query>>();
            builder.RegisterType<Features.Dictionary.SelectOption.Single.QueryValidator>()
                .As<AbstractValidator<Features.Dictionary.SelectOption.Single.Query>>();
            builder.RegisterType<Features.Dictionary.DetailIcgs.List.QueryValidator>()
                .As<AbstractValidator<Features.Dictionary.DetailIcgs.List.Query>>();
            builder.RegisterType<Features.Dictionary.Base.ListTreeNode.QueryValidator>()
                .As<AbstractValidator<Features.Dictionary.Base.ListTreeNode.Query>>();
            builder.RegisterType<Features.Dictionary.DicTariff.List.QueryValidator>()
                .As<AbstractValidator<Features.Dictionary.DicTariff.List.Query>>();

            //Базовые словари
            builder.RegisterType<Features.Dictionary.Base.List.QueryValidator>()
             .As<AbstractValidator<Features.Dictionary.Base.List.Query>>();
            builder.RegisterType<Features.Dictionary.Base.Single.QueryValidator>()
                .As<AbstractValidator<Features.Dictionary.Base.Single.Query>>();

            //Биб. данные
            builder.RegisterType<Features.BibliographicData.List.QueryValidator>()
                .As<AbstractValidator<Features.BibliographicData.List.Query>>();

            //Администрация
            builder.RegisterType<Features.Administration.ApplicationUser.List.QueryValidator>()
                .As<AbstractValidator<Features.Administration.ApplicationUser.List.Query>>();
            builder.RegisterType<Features.Administration.ApplicationUser.Single.QueryValidator>()
                .As<AbstractValidator<Features.Administration.ApplicationUser.Single.Query>>();
            builder.RegisterType<Features.Administration.ApplicationUser.Create.CommandValidator>()
                .As<AbstractValidator<Features.Administration.ApplicationUser.Create.Command>>();
            builder.RegisterType<Features.Administration.ApplicationUser.Update.CommandValidator>()
                .As<AbstractValidator<Features.Administration.ApplicationUser.Update.Command>>();

            //Роли
            builder.RegisterType<Features.Administration.ApplicationRole.List.QueryValidator>()
                .As<AbstractValidator<Features.Administration.ApplicationRole.List.Query>>();
            builder.RegisterType<Features.Administration.ApplicationRole.Single.QueryValidator>()
                .As<AbstractValidator<Features.Administration.ApplicationRole.Single.Query>>();
            builder.RegisterType<Features.Administration.ApplicationRole.Create.CommandValidator>()
                .As<AbstractValidator<Features.Administration.ApplicationRole.Create.Command>>();
            builder.RegisterType<Features.Administration.ApplicationRole.Update.CommandValidator>()
                .As<AbstractValidator<Features.Administration.ApplicationRole.Update.Command>>();
            builder.RegisterType<Features.Administration.ApplicationRole.ListSelect.QueryValidator>()
                .As<AbstractValidator<Features.Administration.ApplicationRole.ListSelect.Query>>();
            builder.RegisterType<Features.Administration.ApplicationRole.ListClaims.QueryValidator>()
                .As<AbstractValidator<Features.Administration.ApplicationRole.ListClaims.Query>>();
            builder.RegisterType<Features.Administration.ApplicationRole.GetRouteStagesTree.QueryValidator>()
                .As<AbstractValidator<Features.Administration.ApplicationRole.GetRouteStagesTree.Query>>();

            //Задачи персонала
            builder.RegisterType<Features.Journal.StaffTasks.List.QueryValidator>()
                .As<AbstractValidator<Features.Journal.StaffTasks.List.Query>>();
            builder.RegisterType<Features.Journal.IntellectualProperties.List.QueryValidator>()
                .As<AbstractValidator<Features.Journal.IntellectualProperties.List.Query>>();

            //Клиенты
            builder.RegisterType<Features.Customer.Single.QueryValidator>()
                .As<AbstractValidator<Features.Customer.Single.Query>>();

            //Оплаты
            builder.RegisterType<Features.Payment.List.QueryValidator>()
                .As<AbstractValidator<Features.Payment.List.Query>>();
            builder.RegisterType<Features.Payment.Invoices.List.QueryValidator>()
                .As<AbstractValidator<Features.Payment.Invoices.List.Query>>();
            builder.RegisterType<Features.Payment.Invoices.Create.CommandValidator>()
                .As<AbstractValidator<Features.Payment.Invoices.Create.Command>>();
            builder.RegisterType<Features.Payment.Uses.Create.CommandValidator>()
                .As<AbstractValidator<Features.Payment.Uses.Create.Command>>();

			//Материалы. Общее
			builder.RegisterType<Features.Materials.Delete.CommandValidator>()
				 .As<AbstractValidator<Features.Materials.Delete.Command>>();
			builder.RegisterType<Features.Materials.WorkflowCreate.CommandValidator>()
				 .As<AbstractValidator<Features.Materials.WorkflowCreate.Command>>();
			builder.RegisterType<Features.Materials.ReplaceAttachment.CommandValidator>()
				 .As<AbstractValidator<Features.Materials.ReplaceAttachment.Command>>();
			builder.RegisterType<Features.Materials.GetDocument.CommandValidator>()
				 .As<AbstractValidator<Features.Materials.GetDocument.Command>>();
			builder.RegisterType<Features.Materials.List.QueryValidator>()
				 .As<AbstractValidator<Features.Materials.List.Query>>();
			builder.RegisterType<Features.Materials.AvailableTypesList.QueryValidator>()
				 .As<AbstractValidator<Features.Materials.AvailableTypesList.Query>>();
			builder.RegisterType<Features.Materials.UserInputFieldsList.QueryValidator>()
				 .As<AbstractValidator<Features.Materials.UserInputFieldsList.Query>>();
		    builder.RegisterType<Features.Materials.GenerateOutgoingNumber.CommandValidator>()
		        .As<AbstractValidator<Features.Materials.GenerateOutgoingNumber.Command>>();
		    builder.RegisterType<Features.Materials.SignDocument.CommandValidator>()
		        .As<AbstractValidator<Features.Materials.SignDocument.Command>>();
		    builder.RegisterType<Features.Materials.ListByOwner.QueryValidator>()
		        .As<AbstractValidator<Features.Materials.ListByOwner.Query>>();

            //Входящие материалы
			builder.RegisterType<Features.Materials.Incoming.Single.QueryValidator>()
				 .As<AbstractValidator<Features.Materials.Incoming.Single.Query>>();
			builder.RegisterType<Features.Materials.Incoming.Update.CommandValidator>()
				 .As<AbstractValidator<Features.Materials.Incoming.Update.Command>>();
			builder.RegisterType<Features.Materials.Incoming.Create.CommandValidator>()
				 .As<AbstractValidator<Features.Materials.Incoming.Create.Command>>();

			//Исходящие материалы
			builder.RegisterType<Features.Materials.Outgoing.Create.CommandValidator>()
				 .As<AbstractValidator<Features.Materials.Outgoing.Create.Command>>();
			builder.RegisterType<Features.Materials.Outgoing.Single.QueryValidator>()
				 .As<AbstractValidator<Features.Materials.Outgoing.Single.Query>>();
			builder.RegisterType<Features.Materials.Outgoing.Update.CommandValidator>()
				 .As<AbstractValidator<Features.Materials.Outgoing.Update.Command>>();

			//Внутренние материалы
			builder.RegisterType<Features.Materials.Internal.Create.CommandValidator>()
				 .As<AbstractValidator<Features.Materials.Internal.Create.Command>>();
			builder.RegisterType<Features.Materials.Internal.Single.QueryValidator>()
				 .As<AbstractValidator<Features.Materials.Internal.Single.Query>>();
			builder.RegisterType<Features.Materials.Internal.Update.CommandValidator>()
				 .As<AbstractValidator<Features.Materials.Internal.Update.Command>>();

            //Субъекты
            builder.RegisterType<Features.Customer.AttachToOwner.CommandValidator>()
                .As<AbstractValidator<Features.Customer.AttachToOwner.Command>>();
            builder.RegisterType<Features.Customer.Create.CommandValidator>()
                .As<AbstractValidator<Features.Customer.Create.Command>>();
            builder.RegisterType<Features.Customer.Delete.CommandValidator>()
                .As<AbstractValidator<Features.Customer.Delete.Command>>();
            builder.RegisterType<Features.Customer.List.QueryValidator>()
                .As<AbstractValidator<Features.Customer.List.Query>>();
            builder.RegisterType<Features.Customer.ListByXinAndName.QueryValidator>()
                .As<AbstractValidator<Features.Customer.ListByXinAndName.Query>>();
            builder.RegisterType<Features.Customer.Update.CommandValidator>()
                .As<AbstractValidator<Features.Customer.Update.Command>>();

			//Экспертный поиск
			builder.RegisterType<Features.Search.List.QueryValidator>()
				 .As<AbstractValidator<Features.Search.List.Query>>();
			builder.RegisterType<Features.Search.RequestList.QueryValidator>()
				 .As<AbstractValidator<Features.Search.RequestList.Query>>();
			builder.RegisterType<Features.Search.ProtectionDocList.QueryValidator>()
				 .As<AbstractValidator<Features.Search.ProtectionDocList.Query>>();
			builder.RegisterType<Features.Search.DocumentList.QueryValidator>()
				 .As<AbstractValidator<Features.Search.DocumentList.Query>>();
			builder.RegisterType<Features.Search.ContractList.QueryValidator>()
				 .As<AbstractValidator<Features.Search.ContractList.Query>>();
			builder.RegisterType<Features.ExpertSearch.TrademarkList.QueryValidator>()
				 .As<AbstractValidator<Features.ExpertSearch.TrademarkList.Query>>();
			builder.RegisterType<Features.ExpertSearch.InventionList.QueryValidator>()
				 .As<AbstractValidator<Features.ExpertSearch.InventionList.Query>>();
			builder.RegisterType<Features.ExpertSearch.UsefulModelList.QueryValidator>()
				 .As<AbstractValidator<Features.ExpertSearch.UsefulModelList.Query>>();
			builder.RegisterType<Features.ExpertSearch.IndustrialDesignList.QueryValidator>()
				 .As<AbstractValidator<Features.ExpertSearch.IndustrialDesignList.Query>>();
			builder.RegisterType<Features.ExpertSearch.ImageSearchList.QueryValidator>()
				.As<AbstractValidator<Features.ExpertSearch.ImageSearchList.Query>>();
		    builder.RegisterType<Features.Search.IntellectualPropertyList.QueryValidator>()
		        .As<AbstractValidator<Features.Search.IntellectualPropertyList.Query>>();

			//Таблица результатов экспертного поиска
			builder.RegisterType<Features.ExpertSearch.Similarities.Create.CommandValidator>()
				  .As<AbstractValidator<Features.ExpertSearch.Similarities.Create.Command>>();
			builder.RegisterType<Features.ExpertSearch.Similarities.List.QueryValidator>()
				.As<AbstractValidator<Features.ExpertSearch.Similarities.List.Query>>();
			builder.RegisterType<Features.ExpertSearch.Similarities.Delete.CommandValidator>()
				.As<AbstractValidator<Features.ExpertSearch.Similarities.Delete.Command>>();
		    builder.RegisterType<Features.ExpertSearch.Similarities.Update.CommandValidator>()
		        .As<AbstractValidator<Features.ExpertSearch.Similarities.Update.Command>>();

            //Календарь
            builder.RegisterType<Features.Administration.Calendar.Delete.CommandValidator>()
                   .As<AbstractValidator<Features.Administration.Calendar.Delete.Command>>();
            builder.RegisterType<Features.Administration.Calendar.Create.CommandValidator>()
                .As<AbstractValidator<Features.Administration.Calendar.Create.Command>>();
            builder.RegisterType<Features.Administration.Calendar.List.QueryValidator>()
                .As<AbstractValidator<Features.Administration.Calendar.List.Query>>();

            builder.RegisterType<Features.Materials.Compare.DocumentsInfoForCompare.QueryValidator>()
                .As<AbstractValidator<Features.Materials.Compare.DocumentsInfoForCompare.Query>>();
            builder.RegisterType<Features.Materials.Compare.DocumentFinished.CommandValidator>()
                .As<AbstractValidator<Features.Materials.Compare.DocumentFinished.Command>>();

            //Договоры
            builder.RegisterType<Create.CommandValidator>()
                .As<AbstractValidator<Create.Command>>();
            builder.RegisterType<Update.CommandValidator>()
                .As<AbstractValidator<Update.Command>>();
            builder.RegisterType<Delete.CommandValidator>()
                .As<AbstractValidator<Delete.Command>>();
            builder.RegisterType<Single.QueryValidator>()
                .As<AbstractValidator<Single.Query>>();
            builder.RegisterType<Register.CommandValidator>()
                .As<AbstractValidator<Register.Command>>();

            // Уведомления
            builder.RegisterType<Features.Notification.List.QueryValidator>()
                .As<AbstractValidator<Features.Notification.List.Query>>();
            builder.RegisterType<Features.Notification.Email.QueryValidator>()
                .As<AbstractValidator<Features.Notification.Email.Query>>();

            //Охранные документы
            builder.RegisterType<Features.ProtectionDoc.Create.CommandValidator>()
                .As<AbstractValidator<Features.ProtectionDoc.Create.Command>>();
            builder.RegisterType<Features.ProtectionDoc.Update.CommandValidator>()
                .As<AbstractValidator<Features.ProtectionDoc.Update.Command>>();
            builder.RegisterType<Features.ProtectionDoc.Single.QueryValidator>()
                .As<AbstractValidator<Features.ProtectionDoc.Single.Query>>();
            builder.RegisterType<Features.ProtectionDoc.GenerateGosNumber.CommandValidator>()
                .As<AbstractValidator<Features.ProtectionDoc.GenerateGosNumber.Command>>();
            builder.RegisterType<Features.ProtectionDoc.WorkflowCreate.CommandValidator>()
                .As<AbstractValidator<Features.ProtectionDoc.WorkflowCreate.Command>>();
        }
    }
}