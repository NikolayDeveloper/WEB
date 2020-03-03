using Iserv.Niis.Integration.OneC.Infrastructure;
using Iserv.Niis.Integration.OneC.Infrastructure.Classes;
using Iserv.Niis.Integration.OneC.Infrastructure.Helpers;
using Iserv.Niis.Integration.OneC.Infrastructure.Interfaces;
using Iserv.Niis.Integration.OneC.Infrastructure.Queries;
using Iserv.Niis.Integration.OneC.Infrastructure.Security;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; ;
using NetCoreDataAccess.UnitOfWork;
//using NetCoreDI;
using Swashbuckle.AspNetCore.Swagger;

namespace Iserv.Niis.Integration.OneC.API
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
        private IConfiguration Configuration { get; }

        /// <summary>
        /// Этот метод вызывается во время выполнения. Используйте этот метод для добавления сервисов в контейнер.
        /// </summary>
        /// <param name="services">Определяет контракт для набора дескрипторов сервисов.</param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSwaggerGen(swaggerGenOptions =>
            {
                swaggerGenOptions.SwaggerDoc("v1", new Info { Title = "Integration 1C API", Version = "v1" });
            });

            //DI соединения с 1C.
            services.AddTransient<IOneCConnection>(s => new OneCConnection(Configuration.GetOneCConnectionString()));

            services
                .AddTransient<DbContext, NiisOneEmptyDbContext>()
                .AddTransient<IExecutor, NiisRepository>()
                .AddTransient<IAmbientContext, AmbientContext>()
                .AddTransient<IUnitOfWork, UnitOfWork>()
                .AddTransient<IObjectResolver, ObjectResolver>();

            #region Регистрируем CQRS Query
            services.AddTransient<GetPaymentsByDateRangeQuery>();
            #endregion

            #region JWT
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options => JwtBearerConfigureOptions.Configure(options, Configuration));
            #endregion

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            var _ = new AmbientContext(services.BuildServiceProvider());
            var __ = new NiisOneCIntegrationAmbientContext(services.BuildServiceProvider());
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
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            #region Swagger для тестирования через api
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Integration 1C API V1");
            });
            #endregion

            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseMvc();
        }
    }
}
