using System;
using Iserv.Niis.Domain.Entities.Dictionaries;

namespace Iserv.Niis.Workflow.Implementations.Request.Stages.TransferedDocumentApplier
{
    public class LogicFactory : ILogicFactory
    {
        private readonly Func<TrademarkLogic> _trademarkFactory;
        private readonly Func<InventionLogic> _inventionFactory;
        private readonly Func<InternationalTrademarkLogic> _internationalTrademarkFactory;
		private readonly Func<UsefulModelLogic> _usefulModelFactory;
		private readonly Func<IndustrialDesignLogic> _industrialDesignLogicFactory;

		public LogicFactory(Func<TrademarkLogic> trademarkFactory, 
		    Func<InventionLogic> inventionFactory, 
		    Func<InternationalTrademarkLogic> internationalTrademarkFactory, 
		    Func<UsefulModelLogic> usefulModelFactory,
		    Func<IndustrialDesignLogic> industrialDesignLogicFactory)
        {
            _trademarkFactory = trademarkFactory;
            _inventionFactory = inventionFactory;
            _internationalTrademarkFactory = internationalTrademarkFactory;
			_usefulModelFactory = usefulModelFactory;
            _industrialDesignLogicFactory = industrialDesignLogicFactory;
        }

        public BaseLogic Create(string protectionDocTypeCode)
        {
            switch (protectionDocTypeCode)
            {
                case DicProtectionDocType.Codes.Trademark: return _trademarkFactory();
                case DicProtectionDocType.Codes.Invention: return _inventionFactory();
                case DicProtectionDocType.Codes.InternationalTrademark: return _internationalTrademarkFactory();
				case DicProtectionDocType.Codes.UsefulModel: return _usefulModelFactory();
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