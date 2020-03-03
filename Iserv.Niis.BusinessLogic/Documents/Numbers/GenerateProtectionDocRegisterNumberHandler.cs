using System.Linq;
using System.Threading.Tasks;
using Iserv.Niis.Common.Codes;
using Iserv.Niis.WorkflowBusinessLogic.SystemCounter;
using Iserv.Niis.DataBridge.Repositories;  using Iserv.Niis.DataBridge.Implementations; //using NetCoreCQRS; .Handlers;

namespace Iserv.Niis.BusinessLogic.Documents.Numbers
{
    public class GenerateProtectionDocRegisterNumberHandler: BaseHandler
    {
        public async Task ExecuteAsync(int documentId)
        {
            var document = await Executor.GetQuery<GetDocumentByIdQuery>().Process(q => q.ExecuteAsync(documentId));
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
                await Executor.GetCommand<UpdateDocumentCommand>().Process(c => c.Execute(document));
            }
        }
    }
}
