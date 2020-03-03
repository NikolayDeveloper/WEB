using System;
using Iserv.Niis.Domain.Entities.Dictionaries;

namespace Iserv.Niis.Workflow.Implementations.Request.Stages.GeneratedNumberApplier
{
    public class LogicFactory : ILogicFactory
    {
        private readonly Func<InternationalTrademarkLogic> _internationalTrademarkFactory;
        private readonly Func<UsefulModelLogic> _usefulModelLogickFactory;
        private readonly Func<IndustrialDesignLogic> _industrialDesignLogicFactory;

        public LogicFactory(Func<InternationalTrademarkLogic> internationalTrademarkFactory,
            Func<UsefulModelLogic> usefulModelLogickFactory,
            Func<IndustrialDesignLogic> industrialDesignLogicFactory)
        {
            _internationalTrademarkFactory = internationalTrademarkFactory;
            _usefulModelLogickFactory = usefulModelLogickFactory;
            _industrialDesignLogicFactory = industrialDesignLogicFactory;
        }

        public BaseLogic Create(string protectionDocTypeCode)
        {
            switch (protectionDocTypeCode)
            {
                case DicProtectionDocType.Codes.InternationalTrademark: return _internationalTrademarkFactory();
                case DicProtectionDocType.Codes.UsefulModel: return _usefulModelLogickFactory();
                case DicProtectionDocType.Codes.IndustrialModel: return _industrialDesignLogicFactory();
                default: return null;
            }
        }
    }
    public interface ILogicFactory
    {
        BaseLogic Create(string protectionDocTypeCode);
    }
}