using Iserv.Niis.DataAccess.EntityFramework;
using Iserv.Niis.DIExtensions;
using Iserv.Niis.Documents.Abstractions;
using Iserv.Niis.Domain.Entities.Security;
using Iserv.Niis.InternalServices.Features.Models;
using Iserv.Niis.Portal.Infrastructure.Filters;
using Iserv.Niis.Portal.Infrastructure.InitialData;
using Iserv.Niis.Portal.Infrastructure.Security;
using Iserv.Niis.Portal.Infrastructure.StartupExtensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Iserv.Niis.Portal
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", false, true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", true);

            if (env.IsDevelopment())
                builder.AddUserSecrets<Startup>();

            builder.AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        private IConfigurationRoot Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IConfiguration>(Configuration);

            #region Отрефакторенные зависимости

            services
                .AddCommonTypesToServiceCollection();

            services.AddTransient<IUserClaimsPrincipalFactory<ApplicationUser>, AppClaimsPrincipalFactory>();
            services.AddTransient<IJwtFactory, JwtFactory>();

            #endregion

            services.AddMvc(options =>
            {
                options.Filters.Add(typeof(GlobalExceptionFilter));
                options.DisableAnonimousAccess();
            });

            services.AddAutoMapperConfiguration();
            services.AddDbContext<NiisWebContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            services.AddIdentity<ApplicationUser, ApplicationRole>(ConfigurationOptions.IdentityOptions).AddEntityFrameworkStores<NiisWebContext>();
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options => ConfigurationOptions.JwtBearerOptions(options, Configuration));
            services.AddAuthorizationPolicies();
            services.AddCors();
            services.Configure<ConfigExternalService>(opt=> Configuration.GetSection("ConfigExternalService").Bind(opt));
            services.AddResponseCompression();

            services.Configure<FormOptions>(o =>
            {
                o.ValueLengthLimit = int.MaxValue;
                o.MultipartBodyLengthLimit = int.MaxValue;
                o.MemoryBufferThreshold = int.MaxValue;
                o.MultipartBodyLengthLimit = int.MaxValue;
            });
        }

        public void Configure(IApplicationBuilder app,
            IHostingEnvironment env,
            ILoggerFactory loggerFactory,
            IApplicationLifetime appLifetime)
        {
            app.UseDefaultFiles();

            app.UseStaticFiles();

            loggerFactory.SetupLogger(appLifetime, Configuration);

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseAuthentication();
            app.SetupCors(Configuration);
            app.SeedInitialData();
            app.UseResponseCompression();
            app.SetupAngularRouting();

            if (env.IsProduction())
                app.ApplicationServices.GetService<NiisWebContext>().Database.Migrate();
        }
    }
}