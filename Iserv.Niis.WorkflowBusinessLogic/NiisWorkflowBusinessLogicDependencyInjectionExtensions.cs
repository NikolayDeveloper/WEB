using Iserv.Niis.Notifications;
using Microsoft.Extensions.DependencyInjection;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Commands;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Handlers;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Queries;
using NetCoreRules;

namespace Iserv.Niis.WorkflowBusinessLogic
{
    public static class NiisWorkflowBusinessLogicDependencyInjectionExtensions
    {
        public static IServiceCollection AddNiisWorkFlowBusinessLogicDependencies(this IServiceCollection serviceCollection)
        {
            serviceCollection.Scan(scan => scan
                .FromAssemblyOf<AssemblyPointerWorkflowBusinessLogic>()
                .AddClasses(classes => classes.AssignableTo<BaseHandler>())
                .AsSelf()
                .WithTransientLifetime());

            serviceCollection.Scan(scan => scan
                .FromAssemblyOf<AssemblyPointerWorkflowBusinessLogic>()
                .AddClasses(classes => classes.AssignableTo<BaseQuery>())
                .AsSelf()
                .WithTransientLifetime());

            serviceCollection.Scan(scan => scan
                .FromAssemblyOf<AssemblyPointerWorkflowBusinessLogic>()
                .AddClasses(classes => classes.AssignableTo<BaseCommand>())
                .AsSelf()
                .WithTransientLifetime());

            serviceCollection.Scan(scan => scan
                .FromAssemblyOf<AssemblyPointerWorkflowBusinessLogic>()
                .AddClasses(classes => classes.AssignableTo<BaseRule>())
                .AsSelf()
                .WithTransientLifetime());

            serviceCollection.Scan(scan => scan
                .FromAssemblyOf<AssemblyPointerNotifications>()
                .AddClasses(classes => classes.AssignableTo<BaseQuery>())
                .AsSelf()
                .WithTransientLifetime());

            serviceCollection.Scan(scan => scan
                .FromAssemblyOf<AssemblyPointerNotifications>()
                .AddClasses(classes => classes.AssignableTo<BaseCommand>())
                .AsSelf()
                .WithTransientLifetime());

            serviceCollection.Scan(scan => scan
                .FromAssemblyOf<AssemblyPointerNotifications>()
                .AddClasses(classes => classes.AssignableTo<BaseHandler>())
                .AsSelf()
                .WithTransientLifetime());

            return serviceCollection;
        }
    }
}
