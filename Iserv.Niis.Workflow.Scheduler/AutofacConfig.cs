using Autofac;
using Iserv.Niis.DataAccess.EntityFramework;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using AutofacSerilogIntegration;
using Iserv.Niis.Business.Implementations;
using Iserv.Niis.Business.Notifications.Implementations;
using Iserv.Niis.Workflow.Implementations.Request;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace Iserv.Niis.Workflow.Scheduler
{
    internal class AutofacConfig
    {
        private static ContainerBuilder _builder;

        internal static IContainer RegisterDependencies()
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", false, true)
                .Build();

            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .CreateLogger();

            _builder = new ContainerBuilder();
            _builder.RegisterLogger();
            _builder
                .Register(c =>
                {
                    var optionsBuilder = new DbContextOptionsBuilder<NiisWebContext>()
                        .UseSqlServer(configuration.GetConnectionString("DefaultConnection"));

                    return new NiisWebContext(optionsBuilder.Options);
                })
                .As<NiisWebContext>().InstancePerLifetimeScope();

            // _builder.RegisterAssemblyTypes(typeof(SchedulerWorker).Assembly);
            RegisterAllNonGenericTypesFromAssembly(typeof(SchedulerWorker).Assembly);
            RegisterAllNonGenericTypesFromAssembly(typeof(TaskRegister).Assembly);
            RegisterAllNonGenericTypesFromAssembly(typeof(NumberGenerator).Assembly);
            RegisterAllNonGenericTypesFromAssembly(typeof(NotificationSender).Assembly);

            //_builder.RegisterType<TaskResolver>().As<ITaskResolver>().InstancePerLifetimeScope();
            //_builder.RegisterType<RequestWorkflowApplier>().As<IRequestWorkflowApplier>();
            //_builder.RegisterType<NumberGenerator>().As<INumberGenerator>();
            //_builder.RegisterType<TaskRegister>().As<ITaskRegister>();
            //_builder.RegisterType<CalendarProvider>().As<ICalendarProvider>();

            return _builder.Build();
        }

        private static void RegisterAllNonGenericTypesFromAssembly(Assembly assembly)
        {
            var types = assembly.GetTypes();
            types.Where(t => !t.IsGenericType)
                .Where(t => !t.IsAbstract)
                .ToList()
                .ForEach(t => _builder.RegisterType(t).AsSelf().AsImplementedInterfaces());
        }
    }
}
