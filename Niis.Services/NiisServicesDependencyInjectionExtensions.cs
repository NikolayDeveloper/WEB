using Iserv.Niis.Services.Implementations;
using Iserv.Niis.Services.Interfaces;
using Iserv.Niis.Utils.Abstractions;
using Iserv.Niis.Utils.Helpers;
using Iserv.Niis.Utils.Implementations;
using Microsoft.Extensions.DependencyInjection;

namespace Iserv.Niis.Services
{
    /// <summary>
    /// Статический класс с методами расширения для инъекции зависимостей сервисов.
    /// </summary>
    public static class NiisServicesDependencyInjectionExtensions
    {
        /// <summary>
        /// Настраивает DI для всех сервисов в сборке <see cref="Services"/>.
        /// </summary>
        /// <param name="services">Сервисы.</param>
        /// <returns>Сервисы.</returns>
        public static IServiceCollection AddNiisServicesDependencies(this IServiceCollection services)
        {

            services.AddScoped<IUploadService, UploadService>();
            services.AddScoped<IProtectionDocService, ProtectionDocService>();
            services.AddScoped<IDocumentService, DocumentService>();
            services.AddScoped<IPaymentService, PaymentService>();
            services.AddScoped<ISearchService, SearchService>();
            services.AddScoped<IImportRequestHelper, ImportRequestHelper>();
            services.AddScoped<IImportPaymentsHelper, ImportPaymentsHelper>();
            services.AddScoped<IImportDocumentsHelper, ImportDocumentsHelper>();
            services.AddScoped<IImportContractsHelper, ImportContractsHelper>();
            services.AddScoped<IBaseImportHelper, BaseImportHelper>();
            services.AddScoped<IGenerateHash, GenerateHash>();
            services.AddScoped<ILkIntergarionHelper, LkIntergarionHelper>();
            services.AddScoped<ISendRequestToLkService, SendRequestToLkService>();

            return services;
        }
    }
}
