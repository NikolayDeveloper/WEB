using System.IO;
using System.Linq;
using System.Reflection;
using Autofac;
using AutofacSerilogIntegration;
using Iserv.Niis.DataAccess.EntityFramework;
using Iserv.Niis.Notifications.Implementations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Serilog;

namespace Iserv.Niis.Notifications.Sender
{
    public class AutofacConfig
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
