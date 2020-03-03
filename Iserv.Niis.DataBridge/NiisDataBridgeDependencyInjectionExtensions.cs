using Iserv.Niis.DataBridge.Implementations;
//using Iserv.Niis.Model.BusinessLogic;
using Microsoft.Extensions.DependencyInjection;



namespace Iserv.Niis.DataBridge
{
    public static class NiisDataBridgeDependencyInjectionExtensions
	{
        public static IServiceCollection AddNiisDataBridgeDependencies(this IServiceCollection serviceCollection)
        {
			//Dependency injection Iserv.Niis.DataBridge
			AddNiisTest(serviceCollection);
            return serviceCollection;
        }

		/// <summary>
		/// Dependency injection Iserv.Niis.DataBridge
		/// </summary>
		/// <param name="serviceCollection"></param>
		private static void AddNiisTest(IServiceCollection serviceCollection)
        {
			serviceCollection.Scan(scan => scan
				.FromAssemblyOf<AssemblyPointerDataBridge>()
				.AddClasses(classes => classes.AssignableTo<BaseQuery>())
				.AsSelf()
				.WithTransientLifetime());

			serviceCollection.Scan(scan => scan
				.FromAssemblyOf<AssemblyPointerDataBridge>()
				.AddClasses(classes => classes.AssignableTo<BaseCommand>())
				.AsSelf()
				.WithTransientLifetime());

			serviceCollection.Scan(scan => scan
				.FromAssemblyOf<AssemblyPointerDataBridge>()
				.AddClasses(classes => classes.AssignableTo<BaseHandler>())
				.AsSelf()
				.WithTransientLifetime());
		}


    }
}
