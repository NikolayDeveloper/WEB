using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Iserv.Niis.Common.Codes;
using Iserv.Niis.WorkflowBusinessLogic.SystemCounter;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Handlers;

namespace Iserv.Niis.WorkflowBusinessLogic.Document
{
    public class GenerateProtectionDocRegisterNumberHandler: BaseHandler
    {
        public void Execute(int documentId)
        {
            var document = Executor.GetQuery<GetDocumentByIdQuery>().Process(q => q.Execute(documentId));
            var protectionDocDocument = document.ProtectionDocs.FirstOrDefault();
            if (protectionDocDocument != null)
            {
                var typeCode = string.Empty;
                switch (protectionDocDocument.ProtectionDoc?.Type?.Code)
                {
                    case DicProtectionDocTypeCodes.ProtectionDocTypeTrademarkCode:
                    case DicProtectionDocTypeCodes.RequestTypeTrademarkCode:
                        typeCode = "ТЗ";
                        break;
                    case DicProtectionDocTypeCodes.ProtectionDocTypeNameOfOriginCode:
                    case DicProtectionDocTypeCodes.RequestTypeNameOfOriginCode:
                        typeCode = "НМПТ";
                        break;
                    case DicProtectionDocTypeCodes.ProtectionDocTypeInventionCode:
                    case DicProtectionDocTypeCodes.RequestTypeInventionCode:
                        typeCode = "ИЗ";
                        break;
                    case DicProtectionDocTypeCodes.ProtectionDocTypeUsefulModelCode:
                    case DicProtectionDocTypeCodes.RequestTypeUsefulModelCode:
                        typeCode = "ПМ";
                        break;
                    case DicProtectionDocTypeCodes.ProtectionDocTypeSelectionAchieveCode:
                    case DicProtectionDocTypeCodes.RequestTypeSelectionAchieveCode:
                        typeCode = "СД";
                        break;
                    case DicProtectionDocTypeCodes.ProtectionDocTypeIndustrialSampleCode:
                    case DicProtectionDocTypeCodes.RequestTypeIndustrialSampleCode:
                        typeCode = "ПО";
                        break;
                }
                var bulletinNumber = protectionDocDocument.ProtectionDoc?.Bulletins?.FirstOrDefault(b => b.IsPublish)
                    ?.Bulletin?.Number;
                var count = Executor.GetHandler<GetNextCountHandler>()
                    .Process(h => h.Execute(bulletinNumber));
                document.DocumentNum = $"{bulletinNumber}.{count}-{typeCode}";
                Executor.GetCommand<UpdateDocumentCommand>().Process(c => c.Execute(document));
            }
        }
    }
}
