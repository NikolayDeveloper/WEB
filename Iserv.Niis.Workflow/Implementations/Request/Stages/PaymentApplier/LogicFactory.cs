using Iserv.Niis.Domain.Entities.Dictionaries;

namespace Iserv.Niis.Workflow.Implementations.Request.Stages.PaymentApplier
{
    public class LogicFactory : ILogicFactory
    {
        private readonly IndustrialDesignLogic _industrialDesignLogic;
        private readonly InternationalTrademarkLogic _internationaltrademark;
        private readonly InventionLogic _invention;
        private readonly TrademarkLogic _trademark;
        private readonly UsefulModelLogic _usefulModel;

        public LogicFactory(TrademarkLogic trademark,
            InventionLogic invention,
            InternationalTrademarkLogic internationaltrademark,
            UsefulModelLogic usefulModel,
            IndustrialDesignLogic industrialDesignLogic)
        {
            _trademark = trademark;
            _invention = invention;
            _internationaltrademark = internationaltrademark;
            _usefulModel = usefulModel;
            _industrialDesignLogic = industrialDesignLogic;
        }

        public BaseLogic Create(string protectionDocTypeCode)
        {
            switch (protectionDocTypeCode)
            {
                case DicProtectionDocType.Codes.Trademark: return _trademark;
                case DicProtectionDocType.Codes.Invention: return _invention;
                case DicProtectionDocType.Codes.InternationalTrademark: return _internationaltrademark;
                case DicProtectionDocType.Codes.UsefulModel: return _usefulModel;
                case DicProtectionDocType.Codes.IndustrialModel: return _industrialDesignLogic;
                default: return null;
            }
        }
    }

    public interface ILogicFactory
    {
        BaseLogic Create(string protectionDocTypeCode);
    }
}