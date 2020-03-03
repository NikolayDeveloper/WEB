using Microsoft.Extensions.DependencyInjection;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Commands;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Handlers;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Queries;

namespace Iserv.Niis.Report
{
    public static class NiisReportBusinessLogicDependencyInjectionExtensions
    {
        public static IServiceCollection AddNiisReportBusinessLogicDependencies(this IServiceCollection serviceCollection)
        {
            AddNiisBusinessLogic(serviceCollection);
            
            return serviceCollection;
        }

        /// <summary>
        /// Dependency injection Iserv.Niis.BusinessLogic
        /// </summary>
        /// <param name="serviceCollection"></param>
        private static void AddNiisBusinessLogic(IServiceCollection serviceCollection)
        {
            serviceCollection.Scan(scan => scan
                .FromAssemblyOf<AssemblyPointerReport>()
                .AddClasses(classes => classes.AssignableTo<BaseQuery>())
                .AsSelf()
                .WithTransientLifetime());
       
            serviceCollection.Scan(scan => scan
                .FromAssemblyOf<AssemblyPointerReport>()
                .AddClasses(classes => classes.AssignableTo<BaseCommand>())
                .AsSelf()
                .WithTransientLifetime());
      
            serviceCollection.Scan(scan => scan
                .FromAssemblyOf<AssemblyPointerReport>()
                .AddClasses(classes => classes.AssignableTo<BaseHandler>())
                .AsSelf()
                .WithTransientLifetime());
        }
    }
}
