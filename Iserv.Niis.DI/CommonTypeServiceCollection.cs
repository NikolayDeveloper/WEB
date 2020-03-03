using Iserv.Niis.Business.Abstract;
using Iserv.Niis.Business.Helpers;
using Iserv.Niis.Business.Implementations;
using Iserv.Niis.Documents.Abstractions;
using Iserv.Niis.Documents.Infrastructure;
using Iserv.Niis.Documents.UserInput.Abstract;
using Iserv.Niis.Documents.UserInput.Implementations;
using Iserv.Niis.FileConverter.Abstract;
using Iserv.Niis.FileConverter.Implementations;
using Iserv.Niis.FileStorage.Abstract;
using Iserv.Niis.FileStorage.Implementations;
using Iserv.Niis.Infrastructure.Abstract;
using Iserv.Niis.Infrastructure.Helpers;
using Iserv.Niis.Infrastructure.Implementations;
using Iserv.Niis.InternalServices.Features.IntegrationGbdFL.Abstractions;
using Iserv.Niis.InternalServices.Features.IntegrationGbdFL.Implementations;
using Iserv.Niis.InternalServices.Features.IntegrationGbdJur.Abstractions;
using Iserv.Niis.InternalServices.Features.IntegrationGbdJur.Implementations;
using Microsoft.Extensions.DependencyInjection;

namespace Iserv.Niis.DI
{
    public static class CommonTypeServiceCollection
    {
        public static IServiceCollection AddCommonTypeServiceCollection(this IServiceCollection services)
        {
            //TODO: Пересмотреть ВЕСЬ процесс создания, всех типов документов, сейчас идет колоссальное дублирование !!! уже не стал инжектить все

            services.AddTransient<ICustomerUpdater, CustomerUpdater>();
            services.AddTransient<IContractCategoryIdentifier, ContractCategoryIdentifier>();

            services.AddTransient<IDicTypeResolver, DicTypeResolver>();
            services.AddTransient<IUserPasswordUpdater, UserPasswordUpdater>();
            services.AddTransient<IRoleClaimsUpdater, RoleClaimsUpdater>();
            services.AddTransient<IRoleRouteStagesUpdater, RoleRouteStagesUpdater>();
            services.AddTransient<ILogoUpdater, LogoUpdater>();
            services.AddTransient<INumberGenerator, NumberGenerator>();
            services.AddTransient<IFileExporter, FileExporter>();
            services.AddTransient<IIntegrationStatusUpdater, IntegrationStatusUpdater>();
            services.AddTransient<IIntegrationDocumentUpdater, IntegrationDocumentUpdater>();
            services.AddTransient<DictionaryHelper>();

            services.AddTransient<ICertificateService, CertificateService>();
            services.AddTransient<IDocumentGeneratorFactory, DocumentGeneratorFactory>();
            services.AddTransient<ITemplateFieldValueFactory, TemplateFieldValueFactory>();
            services.AddTransient<ITemplateUserInputChecker, TemplateUserInputChecker>();
            services.AddTransient<IFileConverter, AsposeFileConverter>();
            services.AddTransient<IAttachmentHelper, AttachmentHelper>();
            services.AddTransient<IFileStorage, MinioFileStorage>();
            services.AddTransient<IDocxTemplateHelper, DocxTemplateHelper>();
            services.AddTransient<ICalendarProvider, CalendarProvider>();
            services.AddTransient<IDocumentsCompare, DocumentsCompare>();

            services.AddTransient<IContractRequestRelationUpdater, ContractRequestRelationUpdater>();

            services.AddTransient<IGbdJuridicalService, GbdJuridicalService>();
            services.AddTransient<IGbdFlService, GbdFlService>();
            services.AddTransient<ILogRecordService, LogRecordService>();
            services.AddTransient<IImportPaymentsFrom1CService, ImportPaymentsFrom1CService>();
			services.AddTransient<IConnectTo1CService, ConnectTo1CService>();

			return services;
        }
    }
}
