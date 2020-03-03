using System;
using Iserv.Niis.Domain.Entities.Dictionaries;

namespace Iserv.Niis.Workflow.Implementations.Request.Stages.SignedDocumentApplier
{
    public class LogicFactory : ILogicFactory
    {
        private readonly TrademarkLogic _trademarkLogic;
        private readonly InventionLogic _inventionLogic;

        public LogicFactory(TrademarkLogic trademarkLogic, InventionLogic inventionLogic)
        {
            _trademarkLogic = trademarkLogic;
            _inventionLogic = inventionLogic;
        }

        public BaseLogic Create(string protectionDocTypeCode)
        {
            switch (protectionDocTypeCode)
            {
                case DicProtectionDocType.Codes.Trademark: return _trademarkLogic;
                case DicProtectionDocType.Codes.Invention: return _inventionLogic;
                default: return null;
            }
        }
    }
    
    public interface ILogicFactory
    {
        BaseLogic Create(string protectionDocTypeCode);
    }
}