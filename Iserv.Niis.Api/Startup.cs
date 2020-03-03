using System;
using AutoMapper;
using Iserv.Niis.Api.HostedServices;
using Iserv.Niis.Api.Infrastructure.InitialData;
using Iserv.Niis.Authentication;
using Iserv.Niis.Business.Abstract;
using Iserv.Niis.Business.Implementations;
using Iserv.Niis.BusinessLogic;
using Iserv.Niis.BusinessLogic.AutoRouteStages;
using Iserv.Niis.DataAccess.EntityFramework;
using Iserv.Niis.DataBridge;
using Iserv.Niis.DataBridge.Repositories;
using Iserv.Niis.DI;
using Iserv.Niis.Domain.Constants;
using Iserv.Niis.Domain.Entities.Security;
using Iserv.Niis.Infrastructure.Infrastructure.Filters;
using Iserv.Niis.Infrastructure.Infrastructure.Security;
using Iserv.Niis.Infrastructure.Security;
using Iserv.Niis.Infrastructure.StartupExtensions;
using Iserv.Niis.InternalServices.Features.Models;
using Iserv.Niis.Report;
using Iserv.Niis.Services;
using Iserv.Niis.WorkflowBusinessLogic;
using Iserv.Niis.WorkflowServices;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; ;
using NetCoreDataAccess.UnitOfWork;
using NetCoreDI;
using NetCoreCQRS;
using Swashbuckle.AspNetCore.Swagger;
using HostingEnvironmentExtensions = Iserv.Niis.Api.Infrastructure.HostingEnvironmentExtensions;

namespace Iserv.Niis.Api
{
	public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAutoMapper(typeof(Model.Mappings.AutoMapperAssemblyPointer).Assembly);

            #region Swagger

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "NIIS API", Version = "v1" });
            });

            #endregion

            #region Глобальные зависимости
            //services
            //.AddDbContextPool<NiisWebContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"),
            //                                                                    sqlServerOptions => sqlServerOptions.CommandTimeout(180)), poolSize: 128)
                services.AddDbContext<NiisWebContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"),
                    sqlServerOptions => sqlServerOptions.CommandTimeout(300)))

                .AddTransient<DbContext, NiisWebContext>()
                //.AddTransient<IExecutor, Executor>()
                .AddTransient<IExecutor, NiisRepository>()
                //.AddScoped<IExecutor, NiisRepository>()
                .AddTransient<IAmbientContext, AmbientContext>()
                .AddTransient<IUnitOfWork, UnitOfWork>()
                .AddTransient<IObjectResolver, ObjectResolver>();
                

            #endregion

            #region Локальные зависимости проекта

            //TODO: необходимо распределить корректно зависимости. Сейчас слишком много мест.
            services
                .AddCommonTypeServiceCollection();

            services
                .AddNiisAuthenticationServiceDependencies()
                .AddNiisBusinessLogicDependencies() // Add all Iserv.Niis.BusinessLogic dependencies
				.AddNiisDataBridgeDependencies() // Add all  Iserv.Niis.DataBridge dependencies
				.AddNiisWorkFlowBusinessLogicDependencies()
                .AddNiisReportBusinessLogicDependencies()
                .AddNiisDIDependencies()
                .AddNiisServicesDependencies();


            services.AddTransient<IAutoRouteStageHelper, AutoRouteStageHelper>();

            services.AddWorkflowServices();

            services.Configure<ConfigExternalService>(opt => Configuration.GetSection("ConfigExternalService").Bind(opt));
            services.AddTransient<IIntegrationOneCApiClient, IntegrationOneCApiClient>();

            #endregion

            #region Authoriazation settings

            services.AddScoped<IUserClaimsPrincipalFactory<ApplicationUser>, AppClaimsPrincipalFactory>();
            services.AddTransient<IJwtFactory, JwtFactory>();
            
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options => ConfigurationOptions.JwtBearerOptions(options, Configuration))
                .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options => options.SessionStore = new MemoryCacheTicketStore());
           
            services.AddIdentity<ApplicationUser, ApplicationRole>(ConfigurationOptions.IdentityOptions).AddEntityFrameworkStores<NiisWebContext>();

            services.AddMvc(options =>
            {
                options.Filters.Add(typeof(GlobalExceptionFilter));

                var authorizePolicy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
                    .Build();
                options.Filters.Add(new AuthorizeFilter(authorizePolicy));
            })
            .AddSessionStateTempDataProvider();

            services.AddAuthorization(options =>
            {
                const string prmName = KeyFor.JwtClaimIdentifier.Permission;

                options.AddPolicy(KeyFor.Policy.HasAccessToJournal, policy => policy.RequireClaim(prmName, KeyFor.Permission.JournalModule));
                options.AddPolicy(KeyFor.Policy.HasAccessToViewStaffTasks, policy => policy.RequireClaim(prmName, KeyFor.Permission.JournalViewStaffTasks));
                options.AddPolicy(KeyFor.Policy.HasAccessToAdministration, policy => policy.RequireClaim(prmName, KeyFor.Permission.AdministrationModule));
            });

            #endregion

            #region Hosted services

            services.AddHostedService<ImportPaymentsFrom1CHostedService>();

            #endregion

            //Создается экземпляр в конце этого метода
            var _ = new AmbientContext(services.BuildServiceProvider());
            var __ = new NiisAmbientContext(services.BuildServiceProvider(), Configuration);
            var ___ = new NiisWorkflowAmbientContext(services.BuildServiceProvider());
        }

        public void Configure(
            IApplicationBuilder app,
            IHostingEnvironment env,
            ILoggerFactory loggerFactory,
            IApplicationLifetime appLifetime
            )
        {
            if (env.IsDevelopment() || HostingEnvironmentExtensions.IsDevelopmentIserv(env) || HostingEnvironmentExtensions.IsDevelopmentNiis(env))
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseDefaultFiles();

            app.UseStaticFiles();

            loggerFactory.SetupLogger(appLifetime, Configuration);

            #region Swagger для тестирования через api

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "NIIS API");
            });

            #endregion

            app.UseAuthentication();

            app.UseMvc();

            app.SetupAngularRouting();

            if (env.IsProduction() || HostingEnvironmentExtensions.IsProductionIserv(env) || HostingEnvironmentExtensions.IsProductionNiis(env) || HostingEnvironmentExtensions.IsStagingNiis(env))
            {
                using (var serviceScope = app.ApplicationServices.CreateScope())
                {
                    serviceScope.ServiceProvider.GetService<NiisWebContext>().Database.Migrate();
                }
            }

            // TODO: раскомментировать, если изменили любой шаблон документа
            // TODO: Добавляет в БД список клэймов с описанием.
            // app.SeedInitialData();
        }
    }
}
