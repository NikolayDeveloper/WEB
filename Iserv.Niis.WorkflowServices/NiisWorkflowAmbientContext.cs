using System;
using Microsoft.Extensions.DependencyInjection;
using NetCoreWorkflow.WorkFlows.Contracts;
using NetCoreWorkflow.WorkFlows.Documents;
using NetCoreWorkflow.WorkFlows.ProtectionDocuments;
using NetCoreWorkflow.WorkFlows.Requests;

namespace Iserv.Niis.WorkflowServices
{
    public class NiisWorkflowAmbientContext
    {
        private readonly IServiceProvider _serviceProvider;

        private static NiisWorkflowAmbientContext _current;

        public static NiisWorkflowAmbientContext Current
        {
            get
            {
                if (_current == null)
                {
                    throw new Exception($"{nameof(NiisWorkflowAmbientContext)} current is null");
                }

                return _current;
            }
        }

        public NiisWorkflowAmbientContext(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _current = this;
        }

        public IWorkflowServiceRequest RequestWorkflowService => _serviceProvider.GetRequiredService<IWorkflowServiceRequest>();
        public IWorkflowServiceContract ContractWorkflowService => _serviceProvider.GetRequiredService<IWorkflowServiceContract>();
        public IWorkflowServiceProtectionDocument ProtectionDocumentWorkflowService => _serviceProvider.GetRequiredService<IWorkflowServiceProtectionDocument>();
        public IWorkflowServiceDocument DocumentWorkflowService => _serviceProvider.GetRequiredService<IWorkflowServiceDocument>();


        #region Рабочие процесы для заявок

        public RequestTradeMarkWorkflow RequestTradeMarkWorkflow => _serviceProvider.GetRequiredService<RequestTradeMarkWorkflow>();
        public RequestInternationalTradeMarkWorkflow RequestInternationalTradeMarkWorkflow => _serviceProvider.GetRequiredService<RequestInternationalTradeMarkWorkflow>();
        public RequestInventionsWorkflow RequestInventionsWorkflow => _serviceProvider.GetRequiredService<RequestInventionsWorkflow>();
        public RequestSelectiveAchievementsWorkflow RequestSelectiveAchievementsWorkFlow => _serviceProvider.GetRequiredService<RequestSelectiveAchievementsWorkflow>();
        public RequestAppellationOfOriginWorkflow RequestAppellationOfOriginWorkflow => _serviceProvider.GetRequiredService<RequestAppellationOfOriginWorkflow>();
        public RequestIndustrialDesignsWorkflow RequestIndustrialDesignsWorkflow => _serviceProvider.GetRequiredService<RequestIndustrialDesignsWorkflow>();
        public RequestUsefulModelWorkflow RequestUsefulModelWorkflow => _serviceProvider.GetRequiredService<RequestUsefulModelWorkflow>();

        #endregion

        #region Рабочие процесы для Охранных окументов
        
        public NmptProtectionDocumentWorkflow ProtectionDocumentAppellationOfOriginWorkflow => _serviceProvider.GetRequiredService<NmptProtectionDocumentWorkflow>();
        public InventionProtectionDocumentWorkflow ProtectionDocumentInventionsWorkflow => _serviceProvider.GetRequiredService<InventionProtectionDocumentWorkflow>();
        public SelectiveAchievementProtectionDocumentWorkflow ProtectionDocumentSelectiveAchievementsWorkflow => _serviceProvider.GetRequiredService<SelectiveAchievementProtectionDocumentWorkflow>();
        public TrademarkProtectionDocumentWorkflow ProtectionDocumentTrademarkWorkflow => _serviceProvider.GetRequiredService<TrademarkProtectionDocumentWorkflow>();
        public UsefulModelProtectionDocumentWorkflow ProtectionDocumentUsefulModelWorkflow => _serviceProvider.GetRequiredService<UsefulModelProtectionDocumentWorkflow>();
        public IndustrialDesignProtectionDocumentWorkflow ProtectionDocumentIndustrialDesignsWorkflow => _serviceProvider.GetRequiredService<IndustrialDesignProtectionDocumentWorkflow>();

        #endregion

        #region Рабочие процесы для Договоров

        public ContractWorkflow ContractWorkflow => _serviceProvider.GetRequiredService<ContractWorkflow>();

        #endregion

        #region Рабочие процесы для Документов

        public CommonDocumentWorkflow CommonDocumentWorkflow => _serviceProvider.GetRequiredService<CommonDocumentWorkflow>();

        #endregion
    }
}
