using System;
using Iserv.Niis.Workflow.Tests.TestData.DicRouteStages.DicRouteStages;
using Iserv.Niis.Workflow.Tests.TestData.TestData.DicTariffs;
using Iserv.Niis.DataAccess.EntityFramework;
using Iserv.Niis.DI;
using Iserv.Niis.DI.DateTimeProviders;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using NetCoreCQRS;
using NetCoreDataAccess.UnitOfWork;
using NetCoreDI;
using NetCoreRules;
using NUnit.Framework;

namespace Iserv.Niis.Workflow.Tests
{
    [TestFixture]
    public class BaseWorkflowTest
    {
        private IExecutor _executor;
        protected IExecutor Executor => _executor ?? (_executor = ServiceProvider.GetRequiredService<IExecutor>());

        protected IServiceCollection ServiceCollection;

        protected ServiceProvider ServiceProvider;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            ServiceCollection = new ServiceCollection()
                .AddEntityFrameworkInMemoryDatabase()
                .AddDbContext<NiisWebContext>(opt => opt.UseInMemoryDatabase($"Niis_test_in_memory_data_base_{Guid.NewGuid()}")
                    .ConfigureWarnings(config => config.Ignore(InMemoryEventId.TransactionIgnoredWarning)))
                .AddScoped<DbContext, NiisWebContext>()
                .AddTransient<IExecutor, Executor>()
                .AddTransient<IAmbientContext, AmbientContext>()
                .AddTransient<IUnitOfWork, UnitOfWork>()
                .AddTransient<IObjectResolver, ObjectResolver>()
                .AddTransient<IRuleExecutor, RuleExecutor>()
                .AddTransient<IDateTimeProvider, DateTimeProvider>()
                .AddSingleton<NiisAmbientContext>();

            AddTestHandlers(ServiceCollection);
            AddTestCommands(ServiceCollection);

            ServiceProvider = ServiceCollection.BuildServiceProvider();
            var _ = new AmbientContext(ServiceProvider);
            var __ = new NiisAmbientContext(ServiceProvider, null);
        }

        protected void FillTestDataBase()
        {
            //НЕ использовать такое в проекте. Это исключтельно для тестового проекта, то бы убедиться что БД создалась
            var dbContext = AmbientContext.Current.Resolver.ResolveObject<NiisWebContext>();
            dbContext.Database.EnsureCreated();
            //---------------------------------------------------------------------------------------------------------

            Executor.GetHandler<FillDicRouteStagesHandler>().Process(h => h.Execute());
            Executor.GetHandler<FillDicTariffsHandler>().Process(h => h.Execute());
        }

        protected void ClearTestDataBase()
        {
            //НЕ использовать такое в проекте. Это исключтельно для тестового проекта, то бы убедиться что БД очистилась
            var dbContext = AmbientContext.Current.Resolver.ResolveObject<NiisWebContext>();
            dbContext.Database.EnsureDeleted();
            //----------------------------------------------------------------------------------------------------------
        }

        private static void AddTestHandlers(IServiceCollection serviceCollection)
        {
            serviceCollection
                .AddTransient<FillDicRouteStagesHandler>()
                .AddTransient<FillDicTariffsHandler>();
        }

        private static void AddTestCommands(IServiceCollection serviceCollection)
        {
            serviceCollection
                .AddTransient<CreateDicRouteStagesCommand>()
                .AddTransient<CreateDicTariffsComand>();
        }
    }
}
