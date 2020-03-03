using System.Configuration;
using System.Reflection;
using AutoMapper;
using Iserv.Niis.Business.Abstract;
using Iserv.Niis.Business.Helpers;
using Iserv.Niis.Business.Implementations;
using Iserv.Niis.DataAccess.EntityFramework;
using Iserv.Niis.DI;
using Iserv.Niis.DI.DateTimeProviders;
using Iserv.Niis.FileStorage.Abstract;
using Iserv.Niis.FileStorage.Implementations;
using Iserv.Niis.Workflow.EventScheduler.Jobs;
using Iserv.Niis.WorkflowBusinessLogic;
using Iserv.Niis.WorkflowServices;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; ;
using NetCoreDataAccess.UnitOfWork;
//using NetCoreDI;
using NetCoreRules;
using NetCoreWorkflow.WorkFlows.Requests;
using Quartz;
using Topshelf;
using Topshelf.Quartz;
using MinioFileStorage = Iserv.Niis.Workflow.EventScheduler.Implementations.MinioFileStorage;

namespace Iserv.Niis.Workflow.EventScheduler
{
    public class Program
    {
        protected static IServiceCollection ServiceCollection;

        protected static ServiceProvider ServiceProvider;

        public static void Main(string[] args)
        {
            ConfigurationDependencies();

            HostFactory.Run(x =>
            {
                x.Service<WorkflowEventSchedulerService>(s =>
                {
                    s.WhenStarted(service => service.OnStart());
                    s.WhenStopped(service => service.OnStop());
                    s.ConstructUsing(() => new WorkflowEventSchedulerService());

                    s.ScheduleQuartzJob(q =>
                        q.WithJob(() =>
                                JobBuilder.Create<WorkflowEventsMonitoringJob>().Build())
                            .AddTrigger(() => TriggerBuilder.Create()
                                .WithSimpleSchedule(b => b                                    
                                    .WithIntervalInSeconds(Properties.Settings.Default.WorkflowsJobInterval * 60)//Интервал запуска джобы, в сеттингах стоит в минутах
                                    .RepeatForever())
                                .Build()));

                    s.ScheduleQuartzJob(q =>
                        q.WithJob(() =>
                                JobBuilder.Create<NotificationMonitoringJob>().Build())
                            .AddTrigger(() => TriggerBuilder.Create()
                                .WithSimpleSchedule(b => b
                                    .WithIntervalInSeconds(Properties.Settings.Default.NotificationsJobInterval * 60)//Интервал запуска джобы, в сеттингах стоит в минутах
                                    .RepeatForever())
                                .Build()));

                    s.ScheduleQuartzJob(q =>
                        q.WithJob(() =>
                                JobBuilder.Create<ProtectionDocSupportJob>().Build())
                            .AddTrigger(() => TriggerBuilder.Create()
                                .WithSimpleSchedule(b => b
                                    .WithIntervalInHours(24)
                                    .RepeatForever())
                                .Build()));
                });

                x.RunAsLocalSystem()
                    .DependsOnEventLog()
                    .StartAutomatically()
                    .EnableServiceRecovery(rc => rc.RestartService(1));

                var serviceName = "NiisService";

                x.SetServiceName(serviceName);
                x.SetDisplayName(serviceName);
                x.SetDescription("Используется для перехода на автоматичекие этапы в системе НИИС");
            });
        }

        private static void ConfigurationDependencies()
        {
            var dbConnectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

            ServiceCollection = new ServiceCollection()
                //.AddDbContextPool<NiisWebContext>(options => options.UseSqlServer(dbConnectionString), poolSize: 8)
                .AddDbContext<NiisWebContext>(options => options.UseSqlServer(dbConnectionString))
                .AddTransient<DbContext, NiisWebContext>()
                .AddTransient<IExecutor, NiisRepository>()
                .AddTransient<IAmbientContext, AmbientContext>()
                .AddTransient<IUnitOfWork, UnitOfWork>()
                .AddTransient<IObjectResolver, ObjectResolver>()
                .AddTransient<IRuleExecutor, RuleExecutor>()
                .AddTransient<IDateTimeProvider, DateTimeProvider>()
                .AddTransient<ICalendarProvider, CalendarProvider>()
                .AddTransient<IIntegrationStatusUpdater, IntegrationStatusUpdater>()
                .AddTransient<IIntegrationDocumentUpdater, IntegrationDocumentUpdater>()
                .AddTransient<IDicTypeResolver, DicTypeResolver>()
                .AddTransient<IFileStorage, MinioFileStorage>()
                .AddTransient<DictionaryHelper>()
                .AddSingleton<NiisAmbientContext>()
                .AddSingleton<RequestAppellationOfOriginWorkflow>()
                .AddNiisWorkFlowBusinessLogicDependencies()
                .AddAutoMapper(mapperConfig => mapperConfig.AddProfiles(typeof(Program).Assembly))
                .AddWorkflowServices();


            ServiceProvider = ServiceCollection.BuildServiceProvider();
            var _ = new AmbientContext(ServiceProvider);
            var __ = new NiisAmbientContext(ServiceProvider, null);
            var ___ = new NiisWorkflowAmbientContext(ServiceProvider);
        }
    }
}
