using Iserv.Niis.Documents;
using Iserv.Niis.Documents.Abstractions;
using Iserv.Niis.Model.BusinessLogic;
using Iserv.Niis.Notifications;
using Microsoft.Extensions.DependencyInjection;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Commands;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Handlers;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Queries;

namespace Iserv.Niis.BusinessLogic
{
    public static class NiisBusinessLogicDependencyInjectionExtensions
    {
        public static IServiceCollection AddNiisBusinessLogicDependencies(this IServiceCollection serviceCollection)
        {
            //Dependency injection Iserv.Niis.BusinessLogic
            AddNiisBusinessLogic(serviceCollection);

            // Dependency injection Iserv.Niis.Documents
            AddNiisDocumentTemplates(serviceCollection);

            // Dependency injection Iserv.Niis.Notifications
            AddNiisNotifications(serviceCollection);

            // Dependency injection Iserv.Niis.Model
            AddNiisModel(serviceCollection);
            return serviceCollection;
        }

        /// <summary>
        /// Dependency injection Iserv.Niis.BusinessLogic
        /// </summary>
        /// <param name="serviceCollection"></param>
        private static void AddNiisBusinessLogic(IServiceCollection serviceCollection)
        {
            serviceCollection.Scan(scan => scan
                .FromAssemblyOf<AssemblyPointerBusinessLogic>()
                .AddClasses(classes => classes.AssignableTo<BaseQuery>())
                .AsSelf()
                .WithTransientLifetime());
       
            serviceCollection.Scan(scan => scan
                .FromAssemblyOf<AssemblyPointerBusinessLogic>()
                .AddClasses(classes => classes.AssignableTo<BaseCommand>())
                .AsSelf()
                .WithTransientLifetime());
      
            serviceCollection.Scan(scan => scan
                .FromAssemblyOf<AssemblyPointerBusinessLogic>()
                .AddClasses(classes => classes.AssignableTo<BaseHandler>())
                .AsSelf()
                .WithTransientLifetime());
        }

        /// <summary>
        /// Dependency injection Iserv.Niis.Documents
        /// </summary>
        /// <param name="serviceCollection"></param>
        private static void AddNiisDocumentTemplates(IServiceCollection serviceCollection)
        {
            serviceCollection.Scan(scan => scan
                .FromAssemblyOf<AssemblyPointerDocuments>()
                .AddClasses(classes => classes.AssignableTo<DocumentGeneratorBase>())
                .AsSelf()
                .WithTransientLifetime());

            serviceCollection.Scan(scan => scan
                .FromAssemblyOf<AssemblyPointerDocuments>()
                .AddClasses(classes => classes.AssignableTo<BaseQuery>())
                .AsSelf()
                .WithTransientLifetime());

            serviceCollection.Scan(scan => scan
                .FromAssemblyOf<AssemblyPointerDocuments>()
                .AddClasses(classes => classes.AssignableTo<TemplateFieldValueBase>())
                .AsSelf()
                .WithTransientLifetime());
        }

        /// <summary>
        /// Dependency injection Iserv.Niis.Notifications
        /// </summary>
        /// <param name="serviceCollection"></param>
        private static void AddNiisNotifications(IServiceCollection serviceCollection)
        {
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
        }


        private static void AddNiisModel(IServiceCollection serviceCollection)
        {
            serviceCollection.Scan(scan => scan
                .FromAssemblyOf<ModelBusinessLogicAssemblyPointer>()
                .AddClasses(classes => classes.AssignableTo<BaseQuery>())
                .AsSelf()
                .WithTransientLifetime());

            serviceCollection.Scan(scan => scan
                .FromAssemblyOf<ModelBusinessLogicAssemblyPointer>()
                .AddClasses(classes => classes.AssignableTo<BaseCommand>())
                .AsSelf()
                .WithTransientLifetime());

            serviceCollection.Scan(scan => scan
                .FromAssemblyOf<ModelBusinessLogicAssemblyPointer>()
                .AddClasses(classes => classes.AssignableTo<BaseHandler>())
                .AsSelf()
                .WithTransientLifetime());
        }
    }
}
