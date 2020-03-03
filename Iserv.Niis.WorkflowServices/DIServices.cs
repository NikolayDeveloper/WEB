using Microsoft.Extensions.DependencyInjection;
using NetCoreRules;
using NetCoreWorkflow.WorkFlows;
using NetCoreWorkflow.WorkFlows.Contracts;
using NetCoreWorkflow.WorkFlows.Documents;
using NetCoreWorkflow.WorkFlows.ProtectionDocuments;
using NetCoreWorkflow.WorkFlows.Requests;

namespace Iserv.Niis.WorkflowServices
{
    public static class DIServices
    {
        public static IServiceCollection AddWorkflowServices(this IServiceCollection serviceCollection)
        {
            #region Регистрируем список рабочих процессов

            serviceCollection.AddSingleton<WorkflowExample>();
            
                #region Заявки

                    serviceCollection.AddSingleton<RequestTradeMarkWorkflow>();
                    serviceCollection.AddSingleton<RequestInternationalTradeMarkWorkflow>();
                    serviceCollection.AddSingleton<RequestInventionsWorkflow>();
                    serviceCollection.AddSingleton<RequestSelectiveAchievementsWorkflow>();
                    serviceCollection.AddSingleton<RequestAppellationOfOriginWorkflow>();
                    serviceCollection.AddSingleton<RequestIndustrialDesignsWorkflow>();
                    serviceCollection.AddSingleton<RequestUsefulModelWorkflow>();

            #endregion

                #region Охранные документы

                serviceCollection.AddSingleton<TrademarkProtectionDocumentWorkflow>();
            serviceCollection.AddSingleton<UsefulModelProtectionDocumentWorkflow>();
            serviceCollection.AddSingleton<SelectiveAchievementProtectionDocumentWorkflow>();
            serviceCollection.AddSingleton<InventionProtectionDocumentWorkflow>();
            serviceCollection.AddSingleton<IndustrialDesignProtectionDocumentWorkflow>();
            serviceCollection.AddSingleton<NmptProtectionDocumentWorkflow>();

            #endregion

            #region Договора

            serviceCollection.AddSingleton<ContractWorkflow>();

                #endregion

                #region Документы

                serviceCollection.AddSingleton<CommonDocumentWorkflow>();

                #endregion

            #endregion

            #region Список сервисов, предоставляющие фабрики для разных групп рабочих процессов(заявки, охранный документ, договора, документы???)

            // сервис, прдоставляющий фабрику по процессам заявок
            serviceCollection.AddTransient<IWorkflowServiceRequest, WorkflowServiceRequest>();
            // сервис, прдоставляющий фабрику по процессам договоров
            serviceCollection.AddTransient<IWorkflowServiceContract, WorkflowServiceContract>();
            // сервис, прдоставляющий фабрику по процессам охранных документов
            serviceCollection.AddTransient<IWorkflowServiceProtectionDocument, WorkflowServiceProtectionDocument>();
            // сервис, прдоставляющий фабрику по процессам документов
            serviceCollection.AddTransient<IWorkflowServiceDocument, WorkflowServiceDocument>();

            #endregion

            serviceCollection.AddTransient<IRuleExecutor, RuleExecutor>();

            return serviceCollection;
        }
    }
}
