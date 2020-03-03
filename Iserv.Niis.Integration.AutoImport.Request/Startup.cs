using AutoMapper;
using Iserv.Niis.Authentication;
using Iserv.Niis.Business.Abstract;
using Iserv.Niis.Business.Helpers;
using Iserv.Niis.Business.Implementations;
using Iserv.Niis.BusinessLogic;
using Iserv.Niis.DataAccess.EntityFramework;
using Iserv.Niis.DI;
using Iserv.Niis.FileStorage.Abstract;
using Iserv.Niis.FileStorage.Implementations;
using Iserv.Niis.Integration.AutoImport.Request.Logger;
using Iserv.Niis.Integration.AutoImport.Request.Services;
using Iserv.Niis.Services.Implementations;
using Iserv.Niis.Services.Interfaces;
using Iserv.Niis.Utils.Abstractions;
using Iserv.Niis.Utils.Implementations;
using Iserv.Niis.WorkflowBusinessLogic;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; ;
using NetCoreDataAccess.UnitOfWork;
//using NetCoreDI;

namespace Iserv.Niis.Integration.AutoImport.Request
{
    public class Startup
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="configuration">Представляет набор свойств конфигурации приложения ключ / значение.</param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        /// <summary>
        /// Представляет набор свойств конфигурации приложения ключ / значение.
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// Этот метод вызывается во время выполнения. Используйте этот метод для добавления сервисов в контейнер.
        /// </summary>
        /// <param name="services">Определяет контракт для набора дескрипторов сервисов.</param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAutoMapper(typeof(Model.Mappings.AutoMapperAssemblyPointer).Assembly);

            #region Глобальные зависимости
            //services
                //.AddDbContextPool<NiisWebContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"), 
                //    sqlServerOptions => sqlServerOptions.CommandTimeout(180)))
                services
                .AddDbContext<NiisWebContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"),
                    sqlServerOptions => sqlServerOptions.CommandTimeout(300)))
                .AddTransient<DbContext, NiisWebContext>()
                .AddTransient<IExecutor, NiisRepository>()
                .AddTransient<IAmbientContext, AmbientContext>()
                .AddTransient<IUnitOfWork, UnitOfWork>()
                .AddTransient<IObjectResolver, ObjectResolver>();

            services
                .AddCommonTypeServiceCollection()
                .AddNiisAuthenticationServiceDependencies()
                .AddNiisBusinessLogicDependencies()
                .AddNiisWorkFlowBusinessLogicDependencies(); // Add all Iserv.Niis.BusinessLogic dependencies
            #endregion
            
            services.AddTransient<IFileStorage, MinioFileStorage>();

            services.AddTransient<IGenerateHash, GenerateHash>();
            services.AddTransient<IEventLogger, EventLogger>();
            //services.AddTransient<IDicTypeResolver, DicTypeResolver>();
            //services.AddTransient<DictionaryHelper>();

            services.AddScoped<IImportRequestHelper, ImportRequestHelper>();
            services.AddScoped<IImportPaymentsHelper, ImportPaymentsHelper>();
            services.AddScoped<IImportDocumentsHelper, ImportDocumentsHelper>();
            services.AddScoped<IImportContractsHelper, ImportContractsHelper>();
            services.AddScoped<IBaseImportHelper, BaseImportHelper>();
            
            services.AddScoped<IAutoImportRequestService, AutoImportRequestService>();
            
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            //Создается экземпляр в конце этого метода
            var _ = new AmbientContext(services.BuildServiceProvider());
            var __ = new NiisAmbientContext(services.BuildServiceProvider(), Configuration);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }

            app.UseMvc();
        }
    }
}
